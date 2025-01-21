using System;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.Payments;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Args;
using Microsoft.EntityFrameworkCore;
using CryptoPtoject.DataBaseInteract;
using CryptoPtoject.DataBaseInteract.Models;

namespace CryptoPtoject.TelegramBots
{
    /// <summary>
    /// класс бота, отвечающего за регистрацию
    /// </summary>
    public class RegistrationBot
    {
        private ITelegramBotClient botClient;
        const string telegramApiToken = "7248550747:AAEVpNpTA7doTh3S8xSCjNRh_c0HK1q2sEQ";
        private InlineKeyboardMarkup inlineKeyboard;
        public RegistrationBot()
        {
            using (var db = new Context()) { db.Database.EnsureCreated(); }
            botClient = new TelegramBotClient(telegramApiToken);
            botClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                new Telegram.Bot.Polling.ReceiverOptions
                {
                    AllowedUpdates = { }
                });
        }
        public async Task SendPumpAndDumpKeyboardAsync(ITelegramBotClient botClient, long chatId)
        {
            var inlineKeyboard = new InlineKeyboardMarkup(new[]
            {
                new []{InlineKeyboardButton.WithCallbackData("Купить подписку на Pump&Dump Screener", "p&d"),}
            });
            await botClient.SendMessage(
               chatId: chatId,
               text: "У вас уже куплена подписка на арбитражника. Рассмотрите другие подписки",
               replyMarkup: inlineKeyboard
           );
        }
        public async Task SendArbitrageKeyboardAsync(ITelegramBotClient botClient, long chatId)
        {
            var inlineKeyboard = new InlineKeyboardMarkup(new[]
            {
                new []{InlineKeyboardButton.WithCallbackData("Купить подписку на Арбитражника", "arb"),}
            });
            await botClient.SendMessage(
               chatId: chatId,
               text: "У вас уже куплена подписка на Pump&Dump скринер. Рассмотрите другие подписки.",
               replyMarkup: inlineKeyboard
           );
        }
        public async Task SendInformationButtons(ITelegramBotClient botClient, long chatId)
        {
            var inlineKeyboard = new InlineKeyboardMarkup(new[]
            {
                new []{InlineKeyboardButton.WithCallbackData("Информация о арбитражнике", "arbInfo"),},
                new []{InlineKeyboardButton.WithCallbackData("Информация о Pump&Dump боте", "p&dInfo"),},
                new []{InlineKeyboardButton.WithCallbackData("Общая информация", "mainInfo"),}
            });
            await botClient.SendMessage(
               chatId: chatId,
               text: "Вся информация о данном проекте",
               replyMarkup: inlineKeyboard
           );
        }
        public async Task SendMainKeyboardAsync(ITelegramBotClient botClient, long chatId)
        {
            var inlineKeyboard = new InlineKeyboardMarkup(new[]
            {
                new []{InlineKeyboardButton.WithCallbackData("Купить подписку на Арбитражника", "arb"),},
                new []{InlineKeyboardButton.WithCallbackData("Купить подписку на Pump&Dump Screener", "p&d"),},
                new []{InlineKeyboardButton.WithCallbackData("Купить подписку на два продукта", "both"),},
                new []{InlineKeyboardButton.WithCallbackData("Посмотреть информацию по подписке", "subInfo"),},
                new []{InlineKeyboardButton.WithCallbackData("Информация о проекте", "help"),}
            });

            await botClient.SendMessage(
                chatId: chatId,
                text: "Выберите действие...",
                replyMarkup: inlineKeyboard
            );
        }
        private async Task SendInvoiceAsync(ITelegramBotClient botClient, long chatId, string typeOfSubscribe)
        {
            var prices = new[]
            {
                new LabeledPrice("Подписка на 1 месяц", 5000000)
            };
            switch(typeOfSubscribe)
            {
                case "arb":
                    await botClient.SendInvoice(
                    chatId: chatId,
                    title: "Подписка на Бота ARBI",
                    description: "Месячная подписка на наш продукт",
                    payload: "ArbiBot",
                    providerToken: "1744374395:TEST:a0c1be34d54c0d92704c",
                    currency: "RUB",
                    prices: prices,
                    startParameter: "start_parameter"
                );
                    break;
                case "p&d":
                    await botClient.SendInvoice(
                    chatId: chatId,
                    title: "Подписка на Pump&Dump скринер",
                    description: "Месячная подписка на наш продукт",
                    payload: "Pump&Dump",
                    providerToken: "1744374395:TEST:a0c1be34d54c0d92704c",
                    currency: "RUB",
                    prices: prices,
                    startParameter: "start_parameter"
                );
                    break;
                case "both":
                    await botClient.SendInvoice(
                    chatId: chatId,
                    title: "Подписка на оба продукта",
                    description: "Месячная подписка на наш продукт",
                    payload: "Both",
                    providerToken: "1744374395:TEST:a0c1be34d54c0d92704c",
                    currency: "RUB",
                    prices: prices,
                    startParameter: "start_parameter"
                );
                    break;
            }
        }
        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
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
                        await SendMainKeyboardAsync(botClient, uId);
                }
            }
            else if (update.Type == UpdateType.CallbackQuery && update.CallbackQuery is not null)
            {
                var callbackQuery = update.CallbackQuery;
                long uId = callbackQuery.From.Id;
                int bothCount;
                int arbitrageCount;
                int pumpAndDumpCount;
                using (var db = new Context())
                {
                    arbitrageCount = db.Users.Include(u => u.SubType).Where(u => u.TelegramId == uId && u.SubTypeId == 1).Count();
                    pumpAndDumpCount = db.Users.Include(u => u.SubType).Where(u => u.TelegramId == uId && u.SubTypeId == 2).Count();
                    bothCount = db.Users.Include(u => u.SubType).Where(u => u.TelegramId == uId && u.SubTypeId == 3).Count();
                    switch (callbackQuery.Data)
                    {
                        case "both":
                            if (bothCount == 1 || (bothCount == 0 && arbitrageCount != 0 && pumpAndDumpCount != 0))
                                await botClient.SendMessage(uId, "У вас уже куплена подписка на оба продукта");
                            else if (bothCount == 0 && arbitrageCount == 0 && pumpAndDumpCount == 0)
                                await SendInvoiceAsync(botClient, uId, "both");
                            else if (bothCount == 0 && arbitrageCount == 0 && pumpAndDumpCount == 1)
                                await SendArbitrageKeyboardAsync(botClient, uId);
                            else if (bothCount == 0 && arbitrageCount == 1 && pumpAndDumpCount == 0)
                                await SendPumpAndDumpKeyboardAsync(botClient, uId);
                            break;
                        case "arb":
                            arbitrageCount = db.Users.Include(u => u.SubType).Where(u => u.TelegramId == uId && u.SubTypeId == 1).Count();
                            if (arbitrageCount == 0 && bothCount == 0)
                                await SendInvoiceAsync(botClient, uId, "arb");
                            else
                                await botClient.SendMessage(uId, "У вас уже куплена подписка на Арбитражника");
                            break; 
                        case "p&d":
                             pumpAndDumpCount = db.Users.Include(u => u.SubType).Where(u => u.TelegramId == uId && u.SubTypeId == 2).Count();
                            if (pumpAndDumpCount == 0 && bothCount == 0)
                                await SendInvoiceAsync(botClient, uId, "p&d");
                            else
                                await botClient.SendMessage(uId, "У вас уже куплена подписка на Pump&Dump скринер");
                            break; 
                        case "help":
                            await SendInformationButtons(botClient, uId);
                            break;
                        case "arbInfo":
                            await botClient.SendMessage(uId, "Бот арбитражник является сигнальным ботом для работе в межбиржевом арбитраже криптовалют.\n" +
                                "Бот ищет связки среды 9 ведущих <b>CEX</b> бирж.\n\n" +
                                "Список бирж:\n" +
                                "1)<b>Binance</b>🔥\n" +
                                "2)<b>BingX</b>🔥\n" +
                                "3)<b>Bitget</b>🔥\n" +
                                "4)<b>Bybit</b>🔥\n" +
                                "5)<b>Coinex</b>🔥\n" +
                                "6)<b>GateIo</b>🔥\n" +
                                "7)<b>Kucoin</b>🔥\n" +
                                "8)<b>Mexc</b>🔥\n" +
                                "9)<b>OKX</b>🔥\n\n" +
                                "Ссылка на бота - @arbi_crypto_mega_bot", ParseMode.Html);
                            break;
                        case "p&dInfo":
                            await botClient.SendMessage(uId, "Pump&Dump скринер - бот, отслеживающий резкие движения на рынке.\n" +
                                "В данный момент скринер работает только с биржей <b>Bybit</b>.\n\n" +
                                "Ссылка на бота - @PandDScreenerbot", ParseMode.Html);
                            break;
                        case "mainInfo":
                            await botClient.SendMessage(uId, $"Мой проект является набором инструментов для заработка в мире криптовалют. " +
                                $"В данный момент есть только два продукта, но в будущем планируется добавление новых инструментов для торговли, " +
                                $"а также расширение функционала уже существующих инструментов.\n\n" +
                                $"Мой канал - https://t.me/it_monkey_cr" +
                                $"Создатель, владелец и разработчик - @senyacm", ParseMode.Html);
                            break;
                        case "subInfo":
                            
                            var users = db.Users.Where(p=>p.TelegramId == uId).ToList();
                            if (users.Count() == 0)
                            {
                                await botClient.SendMessage(uId, $"Вас нет в базе данных бота, купите одну из наших подписок");
                                await SendArbitrageKeyboardAsync(botClient, uId);
                                await SendPumpAndDumpKeyboardAsync(botClient, uId);
                                break;
                            }
                            
                            foreach (var user in users)
                            {
                                await botClient.SendMessage(uId, $"{user.SubTypeId}");
                                switch (user.SubTypeId)
                                {
                                    case 1:
                                        await botClient.SendMessage(uId, $"Ваши данные\n"+
                                        $"Ваш UID: {user.TelegramId}\n" +
                                        $"Тип подписки: <b>Арбитражник</b>\n" +
                                        $"Дата действия подписки: до {user.SubscriptionEnd.ToShortDateString()}\n" +
                                        $"Ссылка на ботa - @arbi_crypto_mega_bot", ParseMode.Html);
                                        break;
                                    case 2:
                                        await botClient.SendMessage(uId,$"Тип подписки: <b>Pump&Dump скринер</b>\n" +
                                        $"Дата действия подписки: до {user.SubscriptionEnd.ToShortDateString()}\n" +
                                        $"Ссылка на бота - @PandDScreenerbot", ParseMode.Html);
                                        break;
                                    case 3:
                                        await botClient.SendMessage(uId, $"<b>Тип подписки: Подписка на все продукты</b>\n" +
                                        $"Дата действия подписки: до {user.SubscriptionEnd.ToShortDateString()}\n" +
                                        $"Pump&Dump скринер - @PandDScreenerbott\n" +
                                        $"Арбитражник - @arbi_crypto_mega_bot", ParseMode.Html);
                                        break;
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            if (update.Type == UpdateType.PreCheckoutQuery)
            {
                var preCheckoutQuery = update.PreCheckoutQuery;
                await botClient.AnswerPreCheckoutQuery(preCheckoutQuery.Id);
            }
            else if (update.Message.SuccessfulPayment != null)
            {
                
                var payment = update.Message.SuccessfulPayment;
                string payload = payment.InvoicePayload;
                long uId = update.Message.From.Id;
                using (var db = new Context())
                {
                    switch (payload.ToLower())
                    {
                        case "arbibot":
                            db.Users.Add(new UserData() { TelegramId = uId, SubscriptionEnd = DateTime.Now.AddMonths(1), SubTypeId = 1 });
                            db.SaveChanges();
                            break;
                        case "pump&dump":
                            db.Users.Add(new UserData() { TelegramId = uId, SubscriptionEnd = DateTime.Now.AddMonths(1), SubTypeId = 2 });
                            db.SaveChanges();
                            break;
                        case "both":
                            db.Users.Add(new UserData() { TelegramId = uId, SubscriptionEnd = DateTime.Now.AddMonths(1), SubTypeId = 3 });
                            db.SaveChanges();
                            break;
                    }
                }
                await botClient.SendMessage(uId,"Оплата прошла успешно! Подписка будет действенна до " + DateTime.Now.AddMonths(1).ToShortDateString());
            }
        }
        private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
