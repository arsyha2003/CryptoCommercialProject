using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot;
using static System.Net.WebRequestMethods;
using Microsoft.EntityFrameworkCore;

namespace ArbiBot
{
    public class PumpAndDumpBot
    {
        private BybitPumpAndDump bb;
        private ITelegramBotClient botClient;
        private string[] pares;
        private CancellationTokenSource cts;
        private Action<string> show;
        public PumpAndDumpBot(Action<string> show)
        {
            cts = new CancellationTokenSource();
            bb = new BybitPumpAndDump();
            botClient = new TelegramBotClient("7836073764:AAE8hacw7Hrpgrd8un0LFG4lkUm_0lLtXc8");
            pares = bb.GetTradePares();
            this.show = show;
        }
        public void StopBot()
        {
            cts.Cancel();
            cts = new CancellationTokenSource();
        }
        public void StartBot()
        {
            pares = bb.GetTradePares();
            using (TradeContext db = new TradeContext())
            {
                ClearTables(db);
                while (true)
                {
                    try
                    {
                        SetInfoToDataBase(db);
                        CompareInfoFromDataBase(db);
                        ClearTables(db);
                    }
                    catch { ClearTables(db); continue; }
                   
                }
            }
        }
        private async void CompareInfoFromDataBase(TradeContext db)
        {
            for (int i = 0; i < db.Signals.Count(); i++)
            {
                if (cts.IsCancellationRequested)
                    return;
                try
                {
                    TimeSpan compareOfTime = (DateTime.Now - db.Signals.ToList()[i].Time);
                    decimal newPrice = bb.GetOrderbook(db.Signals.ToList()[i].Pare);
                    decimal oldPrice = db.Signals.ToList()[i].AvgPrice;

                    decimal spread = bb.CountSpread(newPrice, oldPrice);
                    show.Invoke($"чтение {pares[i]} {spread}");
                    string link = $"https://www.bybit.com/ru-RU/trade/spot/{pares[i].Replace("USDT",string.Empty)}/USDT";
                    if (cts.IsCancellationRequested)
                        return;
                    using(var uDb = new UsersContext())
                    {
                        var users = uDb.Users.Include(u => u.SubType).Select(u => u);
                        foreach (var user in users)
                        {
                            if ((user.SubTypeId == 2 ||
                                    user.SubTypeId == 3) && user.SubscriptionEnd>DateTime.Now)
                            {
                                if (spread >= (decimal)3)
                                {
                                    await botClient.SendMessage(user.TelegramId, $"Изменение по паре {$"<a href=\"{link}\">{pares[i]}</a>"} за " +
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
                        }
                    }
                   
                }
                catch { continue; }
            }
        }
        private void SetInfoToDataBase(TradeContext db)
        {
            for (int i = 0; i < pares.Length; i++)
            {
                show.Invoke($"запись {pares[i]}");
                if (cts.IsCancellationRequested)
                    return;
                decimal price = bb.GetOrderbook(pares[i]);
                bb.FillPareToDb(pares[i], price, DateTime.Now, db);
            }
        }
        private void ClearTables(TradeContext db)
        {
            db.Database.ExecuteSqlRaw("TRUNCATE TABLE Signals");
        }
        
    }
}
