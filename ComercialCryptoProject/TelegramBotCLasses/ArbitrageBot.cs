using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.ApplicationServices;

namespace ArbiBot
{
    public class ArbitrageBot
    {
        private GoArbitrage arb;
        private ITelegramBotClient botClient;
        public decimal range1 = 1;
        public decimal range2 = 5;
        private string[] pares;
        private CancellationTokenSource cts;
        private Action<string> showSpread;
        public ArbitrageBot(Action<string> showSpread)
        {
            arb = new GoArbitrage();
            this.showSpread = showSpread;
            botClient = new TelegramBotClient("8050208272:AAFAeYmM5Jfq61d7BbzEtV_3XFdo7_q71T4");
            cts = new CancellationTokenSource();
        }
        public ArbitrageBot(decimal range1, decimal range2, Action<string> showSpread)
        {
            this.showSpread = showSpread;
            arb = new GoArbitrage(range1, range2);
            botClient = new TelegramBotClient("8050208272:AAFAeYmM5Jfq61d7BbzEtV_3XFdo7_q71T4");
            cts = new CancellationTokenSource();
        }
        public void StopBot()
        {
            cts.Cancel();
            cts = new CancellationTokenSource();
        }
        public void StartBot()
        {
            showSpread.Invoke("Бот запущен");
            pares = arb.GetTradeParesAsync().Result;
            showSpread.Invoke("Пары получеын");
            using (var db = new UsersContext())
            {
                while (true)
                {
                    if (cts.IsCancellationRequested)
                    {
                        showSpread.Invoke("Бот остановлен");
                        return;
                    }
                    foreach (var pare in pares)
                    {
                        try
                        {
                            showSpread.Invoke(pare);
                            if (cts.IsCancellationRequested)
                            {
                                showSpread.Invoke("Бот остановлен");
                                return;
                            }
                            string message = arb.GenerateMessage(pare);
                            if (message != string.Empty)
                            {
                                var users = db.Users.Include(u => u.SubType).ToList();
                                foreach (var user in users)
                                {
                                    if (user.SubType.TypeOfSubscribe == "Arbitrage"  ||
                                        user.SubType.TypeOfSubscribe == "Both" && user.SubscriptionEnd > DateTime.Now) 
                                    {
                                        botClient.SendMessage(user.TelegramId, message, ParseMode.Html);
                                    }
                                }
                            }
                        }
                        catch { continue; }
                    }
                }
            }
        }
    }
}
