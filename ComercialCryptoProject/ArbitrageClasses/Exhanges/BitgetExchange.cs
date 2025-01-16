using BingX.Net.Objects.Models;
using Bitget.Net.Clients;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbiBot
{
    /// <summary>
    /// класс для работы с rest api биржи Bitget
    /// </summary>
    class BitGetExchange : Exchange
    {
        private BitgetRestClient client;
        public BitGetExchange()
        {
            client = new BitgetRestClient();
            makerFeeRate = (decimal)0.1;
            takerFeeRate = (decimal)0.1;
        }
        public override void GetTradePares()
        {
            var tmp = new List<string>();
            var bitgetSymbols = client.SpotApi.ExchangeData.GetSymbolsAsync().Result.Data.ToList();
            tmp = bitgetSymbols
                          .Where(pare => pare.Name.Contains("USDT"))
                          .Select(pare => pare.Name)
                          .Distinct()
                          .ToList();
            tokenList = tmp.ToArray();
        }
        public override void GetFeeRate(string pareName)
        {
            blockChainFullName = new List<string>();
            blockChainIsDepozitable = new List<bool>();
            blockChain = new List<string>();
            blockChainIsWithdrawable = new List<bool>();
            blockChainWithdrawFee = new List<decimal>();
            try
            {
                var assetInfo = client.SpotApi.ExchangeData.GetAssetsAsync().Result;
                if (assetInfo.Success)
                {
                    foreach (var asset in assetInfo.Data.ToList())
                    {
                        if (asset.AssetName == pareName.Replace("USDT", string.Empty))
                        {
                            foreach (var network in asset.Networks)
                            {
                                blockChainFullName.Add(network.Name!);
                                blockChain.Add(network.Name!.ToUpper());
                                blockChainIsWithdrawable.Add(network.Withdrawable);
                                blockChainIsDepozitable.Add(network.Depositable);
                                blockChainWithdrawFee.Add(network.WithdrawFee);
                            }
                        }
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
            var orderBook = client.SpotApi.ExchangeData.GetOrderBookAsync(pareName + "_SPBL", null, 5).Result;
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
        public override string ToString()
        {
            return $"Bitget: {Environment.NewLine}" +
                $"asks price: {asksPrice}{Environment.NewLine}" +
                $"bids price: {bidsPrice}{Environment.NewLine}" +
                $"bidQuantity: {bidQuantity}{Environment.NewLine}" +
                $"avgBuyPrice: {avgBuyPrice}{Environment.NewLine}" +
                $"avgSellPrice: {avgSellPrice}{Environment.NewLine}" +
                $"askQuantity: {askQuantity}{Environment.NewLine}";
        }
    }
}
