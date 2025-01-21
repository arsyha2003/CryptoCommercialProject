using Bybit.Net.Clients;
using CryptoExchange.Net.Authentication;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoProject.Arbitrage.Exchanges
{
    /// <summary>
    /// класс для работы с rest api биржи Bybit
    /// </summary>
    class BybitExchange : Exchange
    {
        //апи обновлено
        const string api = "Rmxk7Kx2tX6lh9V3Td";
        const string apiSecret = "t5ikBA8o0KpOoRPr5t44vQUvTYUpeMebJqGT";

        private BybitRestClient client;
        public BybitExchange() : base()
        {
            client = new BybitRestClient(options => { options.ApiCredentials = new ApiCredentials(api, apiSecret); });
            takerFeeRate = (decimal)0.18;
            makerFeeRate = (decimal)0.1;
        }
        public override void GetTradePares()
        {
            var tmp = new List<string>();
            var bybitSymbols = client.V5Api.ExchangeData.GetSpotSymbolsAsync().Result.Data.List;
            tmp = bybitSymbols
                        .Where(pare=>pare.Name.Contains("USDT"))
                        .Select(pare=>pare.Name)
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
                var assetInfo = client.V5Api.Account.GetAssetInfoAsync().Result;
                if (assetInfo.Success)
                {
                    foreach (var asset in assetInfo.Data.Assets.ToList())
                    {
                        if (asset.Asset == pareName.Replace("USDT", string.Empty))
                        {
                            foreach (var network in asset.Networks.ToList())
                            {
                                blockChainFullName.Add(network.Network);
                                blockChain.Add(network.Network);
                                blockChainWithdrawFee.Add((decimal)network.WithdrawFee!);
                                blockChainIsWithdrawable.Add((bool)network.NetworkWithdraw!);
                                blockChainIsDepozitable.Add((bool)network.NetworkDeposit!);
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
            var orderBook = client.V5Api.ExchangeData.GetOrderbookAsync(Bybit.Net.Enums.Category.Spot, pareName, 5).Result;
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
            return $"Bybit: {Environment.NewLine}" +
                $"asks price: {asksPrice}{Environment.NewLine}" +
                $"bids price: {bidsPrice}{Environment.NewLine}" +
                $"bidQuantity: {bidQuantity}{Environment.NewLine}" +
                $"avgBuyPrice: {avgBuyPrice}{Environment.NewLine}" +
                $"avgSellPrice: {avgSellPrice}{Environment.NewLine}" +
                $"askQuantity: {askQuantity}{Environment.NewLine}";
        }
    }
}