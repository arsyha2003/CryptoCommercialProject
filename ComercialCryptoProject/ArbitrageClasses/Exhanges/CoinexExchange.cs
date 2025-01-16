using CoinEx.Net.Clients;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbiBot
{
    /// <summary>
    /// класс для работы с rest api биржи Coinex
    /// </summary>
    class CoinExExchange : Exchange
    {
        private CoinExRestClient client;
        public CoinExExchange()
        {
            client = new CoinExRestClient();
            makerFeeRate = (decimal)0.3;
            takerFeeRate = (decimal)0.3;
        }
        public override void GetTradePares()
        {
            var tmp = new List<string>();
            var coinexSymbols = client.SpotApi.ExchangeData.GetSymbolsAsync().Result.Data.ToList();
            tmp = coinexSymbols
               .Where(pare => pare.Contains("USDT"))
               .Select(pare => pare)
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

            var assetInfo = client.SpotApi.ExchangeData.GetAssetsAsync(pareName.Replace("USDT", string.Empty)).Result;
            if (assetInfo.Success)
            {
                foreach (var asset in assetInfo.Data.ToList())
                {
                    if (asset.Value.WithdrawFee != 0)
                    {
                        blockChainFullName.Add(asset.Value.Network);
                        blockChain.Add(asset.Value.Network.ToUpper());
                        blockChainIsWithdrawable.Add(asset.Value.CanWithdraw);
                        blockChainWithdrawFee.Add(asset.Value.WithdrawFee);
                        blockChainIsDepozitable.Add(asset.Value.CanDeposit);
                    }
                }
            }
            else return;
        }
        public override void GetOrderBook(string pareName)
        {
            askQuantity = 0;
            bidQuantity = 0;
            avgBuyPrice = 0;
            avgSellPrice = 0;
            var orderBook = client.SpotApi.ExchangeData.GetOrderBookAsync(pareName, 0, 20).Result;
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
            return $"Coinex: {Environment.NewLine}" +
                $"asks price: {asksPrice}{Environment.NewLine}" +
                $"bids price: {bidsPrice}{Environment.NewLine}" +
                $"bidQuantity: {bidQuantity}{Environment.NewLine}" +
                $"avgBuyPrice: {avgBuyPrice}{Environment.NewLine}" +
                $"avgSellPrice: {avgSellPrice}{Environment.NewLine}" +
                $"askQuantity: {askQuantity}{Environment.NewLine}";
        }
    }
}
