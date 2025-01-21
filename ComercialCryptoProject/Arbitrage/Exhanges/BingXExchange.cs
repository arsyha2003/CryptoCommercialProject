using BingX.Net.Clients;
using Bybit.Net.Clients;
using CryptoExchange.Net.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoProject.Arbitrage.Exchanges
{
    /// <summary>
    /// класс для работы с rest api биржи BingX
    /// </summary>
    class BingXExchange : Exchange
    {
        const string api = "9tVuF10Rvpz9HEvWPBaq0e8AmJfhPq89guzq2Z5v5vnv6Xls1smDom8rv41adn8BQov1wC9N6ZqciqjS74g";
        const string apiSecret = "RFyuJavW2iYBWTbblbO9E79rG1HDOfNLIRF4qJ1eUVh3fU5TNs4SjeNWh749za4ql6pOLfjHBNsFsh8vNcAMw";


        private BingXRestClient client;
        public BingXExchange() : base()
        {
            client = new BingXRestClient(options => { options.ApiCredentials = new ApiCredentials(api, apiSecret); });
            takerFeeRate = (decimal)0.1;
            makerFeeRate = (decimal)0.1;
        }
        public override void GetTradePares()
        {
            var tmp = new List<string>();
            var bingXSymbols = client.SpotApi.ExchangeData.GetSymbolsAsync().Result.Data.ToList();
            tmp = bingXSymbols
                              .Where(pare => pare.Name.Contains("USDT"))
                              .Select(pare => pare.Name.Replace("-",string.Empty))
                              .Distinct()
                              .ToList();
            tokenList = tmp.ToArray();
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
                var assetInfo = client.SpotApi.Account.GetAssetsAsync().Result;
                if (assetInfo.Success)
                {
                    foreach (var asset in assetInfo.Data.ToList())
                    {
                        if (asset.Asset == pareName.Replace("USDT", string.Empty))
                        {
                            foreach (var network in asset.Networks)
                            {
                                blockChain.Add(network.Network.ToUpper());
                                blockChainFullName.Add(network.Network);
                                blockChainIsWithdrawable.Add(network.WithdrawEnabled);
                                blockChainWithdrawFee.Add(network.WithdrawFee);
                                blockChainIsDepozitable.Add(network.DepositEnabled);
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
            var orderBook = client.SpotApi.ExchangeData.GetOrderBookAsync(pareName.Replace("USDT", string.Empty) + "-USDT", 5).Result;
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
            return $"BingX: {Environment.NewLine}" +
                $"asks price: {asksPrice}{Environment.NewLine}" +
                $"bids price: {bidsPrice}{Environment.NewLine}" +
                $"bidQuantity: {bidQuantity}{Environment.NewLine}" +
                $"avgBuyPrice: {avgBuyPrice}{Environment.NewLine}" +
                $"avgSellPrice: {avgSellPrice}{Environment.NewLine}" +
                $"askQuantity: {askQuantity}{Environment.NewLine}";
        }
    }
}
