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
        public PumpAndDumpBot()
        {
            botClient = new TelegramBotClient("7836073764:AAE8hacw7Hrpgrd8un0LFG4lkUm_0lLtXc8");
            botClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                new ReceiverOptions
                {
                    AllowedUpdates = { }
                });
            bb = new BybitPumpAndDump();
        }
        public async void StartBot() 
        {
            bb.StartBot();
        }
        public void StopBot() => bb.StopBot();
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
    }
}
