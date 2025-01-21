using Bybit.Net.Clients;
using CryptoExchange.Net.Authentication;
using GateIo.Net.Clients;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoProject.Arbitrage.Exchanges
{
    /// <summary>
    /// класс для работы с rest api биржи GateIo
    /// </summary>
    class GateIoExchange : Exchange
    {
        private GateIoRestClient client;
        const string api = "ea045c2349a75b3e55b5b8405be19fee";
        const string apiSecret = "065ef13b995d1f25493e51c92880d7cd3da5af6c295141e7a4034e407df911dc";
        public GateIoExchange()
        {
            client = new GateIoRestClient(options => { options.ApiCredentials = new ApiCredentials(api, apiSecret); });
            makerFeeRate = (decimal)0.015;
            takerFeeRate = (decimal)0.05;
        }
        public override void GetTradePares()
        {
            var tmp = new List<string>();
            var gateIoSymbols = client.SpotApi.ExchangeData.GetSymbolsAsync().Result.Data.ToList();
            tmp = gateIoSymbols
                   .Where(pare => pare.Name.Replace("_USDT", "USDT").Contains("USDT"))
                  .Select(pare => pare.Name.Replace("_USDT", "USDT"))
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
                var networkList = client.SpotApi.ExchangeData.GetNetworksAsync(pareName.Replace("USDT", string.Empty)).Result;
                var withdrawFee = client.SpotApi.Account.GetWithdrawStatusAsync(pareName.Replace("USDT", string.Empty)).Result;

                if (networkList.Success && withdrawFee.Success)
                {
                    foreach (var network in networkList.Data.ToList())
                    {
                        blockChain.Add(network.Network);
                        blockChainFullName.Add(network.NetworkEn);
                        blockChainIsWithdrawable.Add(!network.IsWithdrawalDisabled);
                        blockChainIsDepozitable.Add(!network.IsDepositDisabled);
                        blockChainWithdrawFee.Add(withdrawFee.Data.ToList().First().WithdrawalFeeFixed);
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
            var orderBook = client.SpotApi.ExchangeData.GetOrderBookAsync(pareName.Replace("USDT", "_") + "USDT", 0, 5).Result;
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
            return $"GateIo: {Environment.NewLine}" +
                $"asks price: {asksPrice}{Environment.NewLine}" +
                $"bids price: {bidsPrice}{Environment.NewLine}" +
                $"bidQuantity: {bidQuantity}{Environment.NewLine}" +
                $"avgBuyPrice: {avgBuyPrice}{Environment.NewLine}" +
                $"avgSellPrice: {avgSellPrice}{Environment.NewLine}" +
                $"askQuantity: {askQuantity}{Environment.NewLine}";
        }
    }
}
