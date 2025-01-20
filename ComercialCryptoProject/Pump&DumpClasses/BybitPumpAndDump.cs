using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bybit.Net.Clients;
using Bybit.Net.Enums;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
namespace ArbiBot
{
    public class BybitPumpAndDump
    {
        public CancellationTokenSource cts;
        private ITelegramBotClient botClient;
        private BybitRestClient client;
        private string[] pares;
        public BybitPumpAndDump()
        {
            botClient = new TelegramBotClient("7836073764:AAE8hacw7Hrpgrd8un0LFG4lkUm_0lLtXc8");
            client = new BybitRestClient();
            pares = GetTradePares();
        }
        public async void StartBot()
        {
            try
            {
                pares = GetTradePares();
                cts = new CancellationTokenSource();
                var token = cts.Token;
                while (true)
                {
                    try
                    {
                        if (token.IsCancellationRequested)
                            return;
                        ClearTables();
                        SetInfoToDataBase(token);
                        CompareInfoFromDataBase(token);
                    }
                    catch { ClearTables(); continue; }
                }
                
            }
            catch { }
        }
        public void StopBot() => cts.Cancel();
        private void SetInfoToDataBase(CancellationToken token)
        {
            using(var db = new Context())
            {
                try
                {
                    for (int i = 0; i < pares.Length; i++)
                    {
                        if (token.IsCancellationRequested)
                            return;
                        decimal price = GetOrderbook(pares[i]);
                        FillPareToDb(pares[i], price, DateTime.Now, db);
                    }
                }
                catch { }
            }
        }
        private async void CompareInfoFromDataBase(CancellationToken token)
        {
            using(var db = new Context())
            {
                for (int i = 0; i < db.BybitInfo.Count(); i++)
                {
                    if (token.IsCancellationRequested)
                        return;
                    try
                    {
                        TimeSpan compareOfTime = (DateTime.Now - db.BybitInfo.ToList()[i].Time);
                        decimal newPrice = GetOrderbook(db.BybitInfo.ToList()[i].Pare);
                        decimal oldPrice = db.BybitInfo.ToList()[i].AvgPrice;

                        decimal spread = CountSpread(newPrice, oldPrice);
                        string link = $"https://www.bybit.com/ru-RU/trade/spot/{pares[i].Replace("USDT", string.Empty)}/USDT";
                        if (token.IsCancellationRequested)
                            return;
                        var users = db.Users.Include(u => u.SubType).Select(u => u);
                        foreach (var user in users)
                        {
                            if ((user.SubTypeId == 2 ||
                                    user.SubTypeId == 3) && user.SubscriptionEnd > DateTime.Now)
                            {
                                try
                                {
                                    if (spread >= (decimal)3)
                                    {
                                        await botClient.SendMessage(user.TelegramId, $"Изменение по паре <a href=\"{link}\">{pares[i]}</a> за " +
                                            $"{compareOfTime.ToString(@"mm\:ss")} {Math.Round(spread, 3)}%🟢\n" +
                                            $"{oldPrice}$ -> {newPrice}$", ParseMode.Html);

                                    }
                                    else if (spread <= (decimal)-3)
                                    {
                                        await botClient.SendMessage(user.TelegramId, $"Изменение по паре <a href=\"{link}\">{pares[i]}</a> за " +
                                            $"{compareOfTime.ToString(@"mm\:ss")} {Math.Round(spread, 3)}%🔴\n" +
                                            $"{oldPrice}$ -> {newPrice}$\n", ParseMode.Html);
                                    }
                                }
                                catch { continue; }
                            }
                        }
                    }
                    catch { continue; }
                }
            }
        }
        private void ClearTables()
        {
            using(var db = new Context())
            {
                db.Database.ExecuteSqlRaw("TRUNCATE TABLE BybitInfo");
            }
        }
        private decimal CountSpread(decimal firstPrice, decimal lastPrice)
        {
            try
            {
                var sp = ((firstPrice - lastPrice) / firstPrice) * 100;
                return sp;
            }
            catch { return 0; }
        }
        private decimal GetOrderbook(string pare)
        {
            try
            {
                var orderBook = client.V5Api.ExchangeData.GetOrderbookAsync(Category.Spot, pare, 5).Result;
                if (orderBook.Success)
                    return orderBook.Data.Bids.ToList()[0].Price;
                else
                    return 0;
            }
            catch { return 0; }
        }
        public string[] GetTradePares()
        {
            try
            {
                var bybitSymbols = client.V5Api.ExchangeData.GetSpotSymbolsAsync().Result.Data.List;
                var tmp = bybitSymbols
                            .Where(pare => pare.Name.Contains("USDT"))
                            .Select(pare => pare.Name)
                            .Distinct()
                            .ToArray();
                return tmp;
            }
            catch { return new string[] { string.Empty }; }
        }
        private bool FillPareToDb(string pare, decimal averagePrice, DateTime time, Context db)
        {
            try
            {
                db.BybitInfo.Add(new BybitPareInfo() { Pare = pare, AvgPrice = averagePrice, Time = time });
                db.SaveChanges();
                return true;
            }
            catch { return false; }
        }
    }
}
