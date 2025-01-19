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
        private BybitRestClient client;
        private string[] pares;
        private Action<string> show;
        public BybitPumpAndDump(Action<string> show)
        {
            client = new BybitRestClient();
            pares = GetTradePares();
            this.show = show;
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
        public async Task StartBot(CancellationToken token, ITelegramBotClient bot)
        {
            pares = GetTradePares();
            ITelegramBotClient botClient = bot;
            using (var db = new Context())
            {
                while (true)
                {
                    try
                    {
                        if (token.IsCancellationRequested)
                            return;
                        ClearTables(db);
                        SetInfoToDataBase(db, token);
                        CompareInfoFromDataBase(db,botClient, token);
                    }
                    catch { ClearTables(db); continue; }
                }
            }
        }
        private void SetInfoToDataBase(Context db, CancellationToken token)
        {
            for (int i = 0; i < pares.Length; i++)
            {
                if (token.IsCancellationRequested)
                    return;
                show.Invoke($"запись {pares[i]}");
                decimal price = GetOrderbook(pares[i]);
                FillPareToDb(pares[i], price, DateTime.Now, db);
            }
        }
        private async void CompareInfoFromDataBase(Context db,ITelegramBotClient botClient, CancellationToken token)
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
                    show.Invoke($"чтение {pares[i]} {spread}");
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
                                    await botClient.SendMessage(user.TelegramId, $"Изменение по паре {$"<a href=\"{link}\">{pares[i]}</a>"} за " +
                                        $"{compareOfTime.ToString(@"mm\:ss")} {Math.Round(spread, 3)}%🔴\n" +
                                        $"{oldPrice}$ -> {newPrice}$\n", ParseMode.Html);
                                }
                            }
                            catch (Exception ex) {show.Invoke(ex.Message); continue; }
                        }
                    }
                }
                catch { continue; }
            }
        }
        private void ClearTables(Context db) => db.Database.ExecuteSqlRaw("TRUNCATE TABLE BybitInfo");
        private decimal CountSpread(decimal firstPrice, decimal lastPrice)
        {
            return ((firstPrice - lastPrice) / firstPrice) * 100;
        }
        private decimal GetOrderbook(string pare)
        {
            var orderBook = client.V5Api.ExchangeData.GetOrderbookAsync(Category.Spot, pare, 5).Result;
            if (orderBook.Success)
                return orderBook.Data.Asks.First().Price;
            else
                return 0;
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
