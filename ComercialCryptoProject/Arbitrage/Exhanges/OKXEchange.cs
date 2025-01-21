using BingX.Net.Clients;
using Kucoin.Net.Objects.Models.Spot;
using Newtonsoft.Json.Linq;
using OKX.Net.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoPtoject.Arbitrage.Exchanges
{
    /// <summary>
    /// класс для работы с rest api биржи OKX
    /// </summary>
    class OKXExchange : Exchange
    {
        private OKXRestClient client;
        const string api = "2b971142-1ae5-481d-9a4a-4f06eb705fcf";
        const string apiSecret = "24EA9989940BA9F744D82C26379EE6B1";
        const string passPhrase = "ArbiTop1$";
        public OKXExchange()
        {
            client = new OKXRestClient(options => { options.ApiCredentials = new OKX.Net.Objects.OKXApiCredentials(api, apiSecret, passPhrase); });
            makerFeeRate = (decimal)0.02;
            takerFeeRate = (decimal)0.05;
        }
        public override void GetTradePares()
        {
            var tmp = new List<string>();
            var okxSymbols = client.UnifiedApi.ExchangeData.GetSymbolsAsync(OKX.Net.Enums.InstrumentType.Spot).Result.Data.ToList();
            tmp = okxSymbols
              .Select(pare => pare.BaseAsset+"USDT")
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
                var assetInfo = client.UnifiedApi.Account.GetAssetsAsync().Result;
                if (assetInfo.Success)
                {
                    foreach (var asset in assetInfo.Data.ToList())
                    {
                        if (asset.Asset == pareName.Replace("USDT", string.Empty))
                        {
                            blockChain.Add(asset.Network.Split('-')[1].ToUpper().Replace(" ", string.Empty));
                            blockChainFullName.Add(asset.Network.Split('-')[1]);
                            blockChainIsWithdrawable.Add(asset.AllowWithdrawal);
                            blockChainIsDepozitable.Add(asset.AllowDeposit);
                            blockChainWithdrawFee.Add(asset.MinimumWithdrawalFee);
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
            var orderBook = client.UnifiedApi.ExchangeData.GetOrderBookAsync(pareName.Replace("USDT", string.Empty) + "-USDT", 5).Result;
            Console.WriteLine(orderBook.Success);
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
            return $"OKX: {Environment.NewLine}" +
                $"asks price: {asksPrice}{Environment.NewLine}" +
                $"bids price: {bidsPrice}{Environment.NewLine}" +
                $"bidQuantity: {bidQuantity}{Environment.NewLine}" +
                $"avgBuyPrice: {avgBuyPrice}{Environment.NewLine}" +
                $"avgSellPrice: {avgSellPrice}{Environment.NewLine}" +
                $"askQuantity: {askQuantity}{Environment.NewLine}";
        }
    }
}
