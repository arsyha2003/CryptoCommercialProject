using GateIo.Net.Objects.Models;
using Kucoin.Net.Clients;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoProject.Arbitrage.Exchanges
{
    /// <summary>
    /// класс для работы с rest api биржи Kucoin
    /// </summary>
    class KucoinExchange : Exchange
    {
        private KucoinRestClient client;
        public KucoinExchange()
        {
            client = new KucoinRestClient();
            takerFeeRate = (decimal)0.16;
            makerFeeRate = (decimal)0.24;
        }
        public override void GetTradePares()
        {
            var tmp = new List<string>();
            var kucoinSymbols = client.SpotApi.ExchangeData.GetSymbolsAsync().Result.Data.ToList();
            tmp = kucoinSymbols
              .Where(pare => pare.Name.Replace("-", string.Empty).Contains("USDT"))
              .Select(pare => pare.Name.Replace("-", string.Empty))
              .Distinct()
              .ToList();

            tokenList = tmp.ToArray();
        }
        public override void GetFeeRate(string pareName)
        {
            blockChainIsDepozitable = new List<bool>();
            blockChainWithdrawFee = new List<decimal>();
            blockChain = new List<string>();
            blockChainFullName = new List<string>();
            blockChainIsWithdrawable = new List<bool>();
            try
            {
                var assetInfo = client.SpotApi.ExchangeData.GetAssetAsync(pareName.Replace("USDT", string.Empty)).Result;
                if (assetInfo.Success)
                {
                    foreach (var network in assetInfo.Data.Networks)
                    {
                        blockChain.Add(network.NetworkName.ToUpper());
                        blockChainFullName.Add(network.NetworkName);
                        blockChainIsDepozitable.Add(network.IsDepositEnabled);
                        blockChainIsWithdrawable.Add(network.IsWithdrawEnabled);
                        blockChainWithdrawFee.Add((decimal)network.WithdrawalMinFee);
                    }
                }
                else return;
            }
            catch { }
        }
        public override void GetOrderBook(string pareName)
        {
            askQuantity = 0;
            bidQuantity = 0;
            avgBuyPrice = 0;
            avgSellPrice = 0;
            try
            {
                var orderBook = client.SpotApi.ExchangeData.GetAggregatedPartialOrderBookAsync(pareName.Replace("USDT", string.Empty) + "-USDT", 20).Result;
                if (orderBook.Success)
                {
                    var bids = orderBook.Data.Bids.ToList();
                    var asks = orderBook.Data.Asks.ToList();
                    List<decimal> tmpBuy = new List<decimal>();
                    List<decimal> tmpSell = new List<decimal>();
                    try
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            bidQuantity += bids[i].Quantity;
                            tmpSell.Add(bids[i].Price);
                        }
                        avgSellPrice = tmpSell.Average();
                        bidsPrice = bids[0].Price;
                    }
                    catch { bidsPrice = 0; bidQuantity = 0; avgBuyPrice = 0; }
                    try
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            askQuantity += asks[i].Quantity;
                            tmpBuy.Add(asks[i].Price);
                        }
                        avgBuyPrice = tmpBuy.Average();
                        asksPrice = asks[0].Price;
                    }
                    catch { asksPrice = 0; askQuantity = 0; avgSellPrice = 0; }
                }
                else
                {
                    bidsPrice = 0;
                    bidQuantity = 0;
                    asksPrice = 0;
                    askQuantity = 0;
                    avgBuyPrice = 0;
                    avgSellPrice = 0;
                }
            }
            catch { }
        }
        public override string ToString()
        {
            return $"Kucoin: {Environment.NewLine}" +
                $"asks price: {asksPrice}{Environment.NewLine}" +
                $"bids price: {bidsPrice}{Environment.NewLine}" +
                $"bidQuantity: {bidQuantity}{Environment.NewLine}" +
                $"avgBuyPrice: {avgBuyPrice}{Environment.NewLine}" +
                $"avgSellPrice: {avgSellPrice}{Environment.NewLine}" +
                $"askQuantity: {askQuantity}{Environment.NewLine}";
        }
    }
}
