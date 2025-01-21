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
using System.Security.Cryptography;
using Telegram.Bot.Polling;
using CryptoProject.Arbitrage;
using CryptoProject.DataBaseInteract;
namespace CryptoProject.TelegramBots
{
    public class ArbitrageBot
    {
        private ArbitrageWorking arb;
        private ITelegramBotClient botClient;
        private CancellationTokenSource cts;
        private CancellationToken token;
        public decimal range1 = 1;
        public decimal range2 = 5;
        private string[] pares;
        public ArbitrageBot()
        {
            arb = new ArbitrageWorking();
            botClient = new TelegramBotClient("8050208272:AAFAeYmM5Jfq61d7BbzEtV_3XFdo7_q71T4");
            botClient.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync,
            new ReceiverOptions
            {
                AllowedUpdates = { }
            });
            cts = new CancellationTokenSource();
        }
        public ArbitrageBot(decimal range1, decimal range2)
        {
            arb = new ArbitrageWorking(range1, range2);
            botClient = new TelegramBotClient("8050208272:AAFAeYmM5Jfq61d7BbzEtV_3XFdo7_q71T4");
            botClient.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync,
            new ReceiverOptions
            {
                AllowedUpdates = { }
            });
            cts = new CancellationTokenSource();
        }
        private async Task HandleUpdateAsync(ITelegramBotClient client, Update update, CancellationToken token)
        {
            if (update == null) return;
            else if (update.Type == UpdateType.Message)
            {
                long uId;
                var message = update.Message;
                uId = message.From.Id;
                using (var db = new Context())
                {
                    if (message.Text == "/start")
                    {
                        var users = db.Users.Include(u => u.SubType).Where(u => u.TelegramId == uId && (u.SubTypeId == 1 || u.SubTypeId == 3)).Select(u => u).ToList();
                        if (users.Count > 0)
                        {
                            var user = users.First();
                            if (user.SubscriptionEnd > DateTime.Now)
                            {
                                await botClient.SendMessage(user.TelegramId, $"Ваша подписка актуальна до {user.SubscriptionEnd.ToShortDateString()}\n" +
                                $"Для подробной информации о арбитражнике и других продуктах - @arbi_reg_bot\n" +
                                $"Мой канал - https://t.me/it_monkey_cr");
                            }
                            else
                            {
                                await botClient.SendMessage(user.TelegramId,$"Ваша подписка закончилась {user.SubscriptionEnd}\n" +
                                    $"Для продления подписки перейдите в бота - @arbi_reg_bot\n" +
                                    $"Мой канал - https://t.me/it_monkey_cr");
                            }
                        }
                        else
                        {
                            await botClient.SendMessage(uId, $"У вас нет подписки на бота арбитражника\n" +
                                $"Для покупки подписки на арбитражника и другие продукты перейдите в бота - @arbi_reg_bot\n" +
                                $"Мой канал - https://t.me/it_monkey_cr");
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
        public async void StartBot()
        {
            cts = new CancellationTokenSource();
            token = cts.Token;
            pares = arb.GetTradeParesAsync().Result;
            using (var db = new Context())
            {
                while (true)
                {
                    if (token.IsCancellationRequested)
                        return;
                    foreach (var pare in pares)
                    {
                        try
                        {
                            if (token.IsCancellationRequested)
                                return;
                            string message = arb.GenerateMessage(pare);
                            if (message != string.Empty)
                            {
                                var users = db.Users.Include(u => u.SubType).Select(u => u);
                                foreach (var user in users)
                                {
                                    if ((user.SubTypeId == 1 ||
                                        user.SubTypeId == 3) && user.SubscriptionEnd > DateTime.Now)
                                    {
                                        await botClient.SendMessage(user.TelegramId, message, ParseMode.Html);
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
