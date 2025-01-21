using CryptoProject.Arbitrage.Exchanges;
using HTX.Net.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HTX.Net;
using HTX.Net.Clients;
using CryptoExchange.Net.Authentication;
namespace CryptoProject.Arbitrage.Exhanges
{
    class HTXExchange : Exchange
    {
        const string api = "0a124939-e29d8182-dqnh6tvdf3-b3ca6";
        const string apiSecret = "83552316-a697473a-fa19ad94-a579a";
        private HTXRestClient client;
        public HTXExchange()
        {
            takerFeeRate = (decimal)0.2;
            makerFeeRate = (decimal)0.2;
            client = new HTXRestClient(options => { options.ApiCredentials = new ApiCredentials(api, apiSecret); });
        }
        public override void GetOrderBook(string pareName)
        {
            askQuantity = 0;
            bidQuantity = 0;
            avgBuyPrice = 0;
            avgSellPrice = 0;
            var orderBook = client.SpotApi.ExchangeData.GetOrderBookAsync(pareName, 0, 5).Result;
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
                        bidQuantity += bids[i].Quantity * bids[i].Price;
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
                        askQuantity += asks[i].Quantity * asks[i].Price;
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
        public override void GetFeeRate(string pareName)
        {
            blockChainFullName = new List<string>();
            blockChainIsDepozitable = new List<bool>();
            blockChainWithdrawFee = new List<decimal>();
            blockChain = new List<string>();
            blockChainIsWithdrawable = new List<bool>();
            try
            {
                var assetInfo = client.SpotApi.ExchangeData.GetAssetsAndNetworksAsync().Result;
                if (assetInfo.Success)
                {
                    foreach (var asset in assetInfo.Data.ToList())
                    {
                        if (asset.Asset == pareName.Replace("USDT", string.Empty).ToLower())
                        {
                            foreach (var network in asset.Networks.ToList())
                            {
                                blockChain.Add(network.DisplayName);
                                blockChainFullName.Add(network.BaseNetwork);
                                blockChainWithdrawFee.Add(network.MaxTransactFeeWithdraw);

                                if (network.WithdrawStatus == NetworkStatus.Allowed)
                                    blockChainIsWithdrawable.Add(true);
                                else if (network.WithdrawStatus == NetworkStatus.Prohibited)
                                    blockChainIsWithdrawable.Add(false);
                                if (network.DepositStatus == NetworkStatus.Allowed)
                                    blockChainIsDepozitable.Add(true);
                                else if (network.DepositStatus == NetworkStatus.Prohibited)
                                    blockChainIsDepozitable.Add(false);
                            }
                        }
                    }
                }
                else return;
            }
            catch { }
        }
        public override void GetTradePares()
        {
            throw new NotImplementedException();
        }
    }
}