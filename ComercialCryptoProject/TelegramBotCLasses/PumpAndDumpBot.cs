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
        public CancellationTokenSource cts;
        private Action<string> show;
        public PumpAndDumpBot(Action<string>show)
        {
            this.show = show;
            cts = new CancellationTokenSource();
            botClient = new TelegramBotClient("7836073764:AAE8hacw7Hrpgrd8un0LFG4lkUm_0lLtXc8");
            bb = new BybitPumpAndDump(show);
            botClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                new ReceiverOptions
                {
                    AllowedUpdates = { }
                });
        }
        public async void StartBot() 
        {
            cts = new CancellationTokenSource();
            await Task.Run(() => bb.StartBot(cts.Token, botClient));
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
        
    }
}
