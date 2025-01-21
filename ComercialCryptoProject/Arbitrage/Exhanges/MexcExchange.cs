using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CryptoExchange.Net.Authentication;
using Mexc.Net.Clients;
using OKX.Net.Clients;
namespace CryptoPtoject.Arbitrage.Exchanges
{
    /// <summary>
    /// класс для работы с rest api биржи Mexc
    /// </summary>
    class MexcExchange : Exchange
    {
        const string api = "mx0vglLO08i0p2KHUY";
        const string apiSecret = "c2a209ae4ecc47608a1a1566462d6d43";
        private MexcRestClient client;
        public MexcExchange() : base()
        {
            takerFeeRate = (decimal)0.01;
            makerFeeRate = (decimal)0;
            client = new MexcRestClient(options => { options.ApiCredentials = new ApiCredentials(api, apiSecret); });
        }
        public override void GetTradePares()
        {
            var tmp = new List<string>();
            var mexcSymbols = client.SpotApi.ExchangeData.GetApiSymbolsAsync().Result.Data.ToList();
            tmp = mexcSymbols
              .Select(pare => pare)
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
                var networkList = client.SpotApi.Account.GetUserAssetsAsync().Result;
                if (networkList.Success)
                {
                    foreach (var network in networkList.Data.ToList())
                    {
                        if (network.Asset == pareName.Replace("USDT", string.Empty))
                        {
                            foreach (var a in network.Networks.ToList())
                            {
                                blockChainFullName.Add(a.Network);
                                blockChain.Add(a.Network);
                                blockChainWithdrawFee.Add((decimal)a.WithdrawFee);
                                blockChainIsWithdrawable.Add(a.WithdrawEnabled);
                                blockChainIsDepozitable.Add(a.DepositEnabled);
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
            return $"Mexc: {Environment.NewLine}" +
                $"asks price: {asksPrice}{Environment.NewLine}" +
                $"bids price: {bidsPrice}{Environment.NewLine}" +
                $"bidQuantity: {bidQuantity}{Environment.NewLine}" +
                $"avgBuyPrice: {avgBuyPrice}{Environment.NewLine}" +
                $"avgSellPrice: {avgSellPrice}{Environment.NewLine}" +
                $"askQuantity: {askQuantity}{Environment.NewLine}";
        }
    }
}
