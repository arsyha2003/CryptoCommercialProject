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
using Telegram.Bot.Polling;

namespace ArbiBot
{
    public class PumpAndDumpBot
    {
        private BybitPumpAndDump bb;
        private ITelegramBotClient botClient;
        private string[] pares;
        private CancellationTokenSource cts;
        public PumpAndDumpBot()
        {
            cts = new CancellationTokenSource();
            bb = new BybitPumpAndDump();
            botClient = new TelegramBotClient("7836073764:AAE8hacw7Hrpgrd8un0LFG4lkUm_0lLtXc8");
            pares = bb.GetTradePares();
            botClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                new ReceiverOptions
                {
                    AllowedUpdates = { }
                });
        }
        private async Task HandleUpdateAsync(ITelegramBotClient client, Update update, CancellationToken token)
        {
            if (update == null) return;
            else if (update.Type == UpdateType.Message)
            {
                long uId;
                var message = update.Message;
                uId = message.From.Id;
                using (var db = new UsersContext())
                {
                    if (message.Text == "/start")
                    {
                        var users = db.Users.Include(u => u.SubType).Where(u => u.TelegramId == uId && (u.SubTypeId == 2 || u.SubTypeId == 3)).Select(u => u).ToList();
                        if (users.Count > 0)
                        {
                            var user = users.First();
                            if (user.SubscriptionEnd > DateTime.Now)
                            {
                                await botClient.SendMessage(user.TelegramId, $"Ваша подписка актуальна до {user.SubscriptionEnd.ToShortDateString()}\n" +
                                $"Для подробной информации о Pump&Dump скринере и других продуктах - @PandDScreenerbot");
                            }
                            else
                            {
                                await botClient.SendMessage(user.TelegramId, $"Ваша подписка закончилась {user.SubscriptionEnd}\n" +
                                    $"Для продления подписки перейдите в бота - @PandDScreenerbot");
                            }
                        }
                        else
                        {
                            await botClient.SendMessage(uId, $"У вас нет подписки на Pump&Dump скринер\n" +
                                $"Для покупки подписки на скринер и другие продукты перейдите в бота - @arbi_reg_bot");
                        }
                    }
                }

            };
        }
        private Task HandleErrorAsync(ITelegramBotClient client, Exception exception, HandleErrorSource source, CancellationToken token)
        {
            return Task.CompletedTask;
        }
        public void StopBot()
        {
            cts.Cancel();
        }
        public async Task StartBot()
        {
            cts = new CancellationTokenSource();
            var token = cts.Token;
            pares = bb.GetTradePares();
            using (TradeContext db = new TradeContext())
            {
                while (true)
                {
                    try
                    {
                        if (token.IsCancellationRequested)
                            return;
                        ClearTables(db);
                        SetInfoToDataBase(db, token);
                        CompareInfoFromDataBase(db, token);
                    }
                    catch { ClearTables(db); continue; }
                }
            }
        }
        private async void CompareInfoFromDataBase(TradeContext db, CancellationToken token)
        {
            for (int i = 0; i < db.Signals.Count(); i++)
            {
                if (token.IsCancellationRequested)
                    return;
                try
                {
                    TimeSpan compareOfTime = (DateTime.Now - db.Signals.ToList()[i].Time);
                    decimal newPrice = bb.GetOrderbook(db.Signals.ToList()[i].Pare);
                    decimal oldPrice = db.Signals.ToList()[i].AvgPrice;

                    decimal spread = bb.CountSpread(newPrice, oldPrice);
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
        private void SetInfoToDataBase(TradeContext db, CancellationToken token)
        {
            for (int i = 0; i < pares.Length; i++)
            {
                if (token.IsCancellationRequested)
                    return;
                decimal price = bb.GetOrderbook(pares[i]);
                bb.FillPareToDb(pares[i], price, DateTime.Now, db);
            }
        }
        private void ClearTables(TradeContext db) => db.Database.ExecuteSqlRaw("TRUNCATE TABLE Signals");
    }
}
