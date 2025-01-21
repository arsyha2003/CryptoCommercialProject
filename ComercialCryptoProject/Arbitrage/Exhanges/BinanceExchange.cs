using Binance.Net.Clients;
using CryptoExchange.Net.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoProject.Arbitrage.Exchanges
{
    /// <summary>
    /// класс для работы с rest api биржи Binance
    /// </summary>
    class BinanceExchange : Exchange
    {
        const string api = "1MYgj9lad0UBhGzuTQI7F0PSCSVQGVUXntLVtEoOtooU2nfbYpB0Qp4OH2gG31dW";
        const string apiSecret = "iYpcidDkBabwHbAk6XLzwYrKVeKqkq91YQ20YI21r6XhhyVXmpqBlKlfZGP0PV5d";
        private BinanceRestClient client;
        public BinanceExchange()
        {
            client = new BinanceRestClient(options => { options.ApiCredentials = new ApiCredentials(api, apiSecret); });
            makerFeeRate = (decimal)0.015;
            takerFeeRate = (decimal)0.015;
        }
        public override void GetTradePares()
        {
            //TODO
        }
        public override void GetFeeRate(string pareName)
        {
            blockChainFullName = new List<string>();
            blockChainIsDepozitable = new List<bool>();
            blockChainWithdrawFee = new List<decimal>();
            blockChain = new List<string>();
            blockChainIsWithdrawable = new List<bool>();
            var assetInfo = client.SpotApi.Account.GetUserAssetsAsync().Result;
            try
            {
                if (assetInfo.Success)
                {
                    foreach (var asset in assetInfo.Data.ToList())
                    {
                        if (asset.Asset == pareName.Replace("USDT", string.Empty))
                        {
                            foreach (var network in asset.NetworkList)
                            {
                                blockChainFullName.Add(network.Network);
                                blockChain.Add(network.Network.ToUpper());
                                blockChainWithdrawFee.Add(network.WithdrawFee);
                                blockChainIsWithdrawable.Add(network.WithdrawEnabled);
                                blockChainIsDepozitable.Add(network.DepositEnabled);

                            }
                        }
                    }
                }
            }
            catch { }
        }
        public override void GetOrderBook(string pareName)
        {
            askQuantity = 0;
            bidQuantity = 0;
            avgBuyPrice = 0;
            avgSellPrice = 0;
            var orderBook = client.SpotApi.ExchangeData.GetOrderBookAsync(pareName, 5).Result;
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
            return $"Binance: {Environment.NewLine}" +
                $"asks price: {asksPrice}{Environment.NewLine}" +
                $"bids price: {bidsPrice}{Environment.NewLine}" +
                $"bidQuantity: {bidQuantity}{Environment.NewLine}" +
                $"avgBuyPrice: {avgBuyPrice}{Environment.NewLine}" +
                $"avgSellPrice: {avgSellPrice}{Environment.NewLine}" +
                $"askQuantity: {askQuantity}{Environment.NewLine}";
        }
    }
}
