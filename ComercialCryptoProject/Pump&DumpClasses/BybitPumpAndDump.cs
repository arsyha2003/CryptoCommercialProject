using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bybit.Net.Clients;
namespace ArbiBot
{
    public class BybitPumpAndDump
    {
        private BybitRestClient client;
        public BybitPumpAndDump()
        {
            client = new BybitRestClient();
        }
        public string[] GetTradePares()
        {
            var bybitSymbols = client.V5Api.ExchangeData.GetSpotSymbolsAsync().Result.Data.List;
            var tmp = bybitSymbols
                        .Where(pare => pare.Name.Contains("USDT"))
                        .Select(pare => pare.Name)
                        .Distinct()
                        .ToArray();
            return tmp;
        }
        public decimal GetOrderbook(string pare)
        {
            var orderBook = client.V5Api.ExchangeData.GetOrderbookAsync(Bybit.Net.Enums.Category.Spot, pare, 5).Result;
            if (orderBook.Success)
            {
                var ask = orderBook.Data.Asks.First().Price;
                return ask;
            }
            else return 0;
        }
        public bool FillPareToDb(string pare, decimal averagePrice, DateTime time, TradeContext db)
        {
            try
            {
                ExchangeInfo tmp = new ExchangeInfo() { Pare = pare, AvgPrice = averagePrice,Time = time };
                db.Signals.Add(tmp);
                db.SaveChanges();
                return true;
            }
            catch { return false; }
        }
        public decimal CountSpread(decimal firstPrice, decimal lastPrice)
        {
            return ((firstPrice - lastPrice) / firstPrice) * 100;
        }
    }
}
