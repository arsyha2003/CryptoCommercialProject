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
        private ITelegramBotClient botClient;
        public CancellationTokenSource cts;
        private BybitRestClient client;
        private string[] pares;
        public BybitPumpAndDump()
        {
            botClient = new TelegramBotClient("7836073764:AAE8hacw7Hrpgrd8un0LFG4lkUm_0lLtXc8");
            client = new BybitRestClient();
            pares = GetTradePares();
        }
        public async Task StartBot()
        {
            cts = new CancellationTokenSource();
            var token = cts.Token;
            try
            {
                pares = GetTradePares();
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
                    catch(Exception ex) 
                    { 
                        ClearTables();
                        continue; 
                    }
                }
                
            }
            catch { }
        }
        public void StopBot() => cts.Cancel();
        private void SetInfoToDataBase(CancellationToken token)
        {
            try
            {
                for (int i = 0; i < pares.Length; i++)
                {
                    if (token.IsCancellationRequested)
                        return;
                    decimal price = GetOrderbook(pares[i]);
                    FillPareToDb(pares[i], price, DateTime.Now);
                }
            }
            catch { }
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
                    catch(Exception ex) { continue; }
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
            catch (Exception ex) { return 0; }
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
            catch (Exception ex) { return 0; }
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
            catch(Exception ex) { return new string[] { string.Empty }; }
        }
        private bool FillPareToDb(string pare, decimal averagePrice, DateTime time)
        {
            using(var db = new Context())
            {
                try
                {
                    db.BybitInfo.Add(new BybitPareInfo() { Pare = pare, AvgPrice = averagePrice, Time = time });
                    db.SaveChanges();
                    return true;
                }
                catch(Exception ex) { }
            }
            return false;
        }
    }
}
