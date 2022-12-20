using System.Threading.Tasks;
using System.Threading;
using System;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using System.Collections.Generic;
using TelegramBot.Models;
using TelegramBot.Helpers;
using System.Linq;

namespace TelegramBot
{
    public class BotEngine
    {
        private readonly TelegramBotClient _botClient;

        public BotEngine(TelegramBotClient botClient)
        {
            _botClient = botClient;
        }

        public async Task ListenForMessagesAsync()
        {
            var cts = new CancellationTokenSource();

            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = Array.Empty<UpdateType>() // receive all update types
            };
            _botClient.StartReceiving(
                updateHandler: HandleUpdateAsync,
                pollingErrorHandler: HandlePollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: cts.Token
            );

            var me = await _botClient.GetMeAsync();

            Console.WriteLine($"Start listening for @{me.Username}");
            Console.ReadLine();
        }

        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type == UpdateType.Message && update?.Message?.Text != null)
            {
                await HandleMessageAsync(botClient, update.Message, cancellationToken);
                return;
            }
            if (update.Type == UpdateType.CallbackQuery)
            {
                await HandleCallbackQuery(botClient, update.CallbackQuery);
                return;
            }
        }

        private async Task HandleMessageAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {
            if (message.Text == "/start" || message.Text == "/home")
            {
                ReplyKeyboardMarkup keyboardMarkup = new ReplyKeyboardMarkup(new[]
                {
                    new KeyboardButton[] {"Топ +% 24H", "Топ -% 24H" },
                    new KeyboardButton[] {"Futures Long/Short" }
                })
                {
                    ResizeKeyboard = true
                };

                await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "Choose",
                    replyMarkup: keyboardMarkup,
                    cancellationToken: cancellationToken
                    );
                return;
            }
            else if (message.Text == "Топ +% 24H")
            {
                string result = CryptoMethods.GetPercentChange(false);
                //Console.WriteLine(Requests.GetRequestCoinGlass("https://open-api.coinglass.com/public/v2/perpetual_market","BTC"));
                await SendMessageAsync(botClient, message, result, cancellationToken);
                return;
            }
            else if (message.Text == "Топ -% 24H")
            {
                string result = CryptoMethods.GetPercentChange(true);
                await SendMessageAsync(botClient, message, result, cancellationToken);
                return;
            }else if(message.Text == "Futures Long/Short")
            {
                InlineKeyboardMarkup keyboardMarkup = new InlineKeyboardMarkup(new[]
                {
                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithCallbackData("BTC"),
                        InlineKeyboardButton.WithCallbackData("ETH"),
                        InlineKeyboardButton.WithCallbackData("BNB"),

                    },
                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithCallbackData("XRP"),
                        InlineKeyboardButton.WithCallbackData("MATIC"),
                        InlineKeyboardButton.WithCallbackData("DOT"),

                    },
                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithCallbackData("SOL"),
                        InlineKeyboardButton.WithCallbackData("ATOM"),
                        InlineKeyboardButton.WithCallbackData("ALGO"),

                    }
                });
                await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "Choose Crypto",
                    replyMarkup: keyboardMarkup
                    );
                return;
            }

            await SendMessageAsync(botClient, message, "Не знаю что ответить", cancellationToken);
        }
        private async Task HandleCallbackQuery(ITelegramBotClient botClient,CallbackQuery callbackQuery)
        {
            string result = CryptoMethods.GetPercentShortLong(callbackQuery.Data.ToString());
            await botClient.SendTextMessageAsync(
                chatId: callbackQuery.Message.Chat.Id,
                text: result
                );
        }

        private async Task SendMessageAsync(ITelegramBotClient botClient, Message message, string text, CancellationToken cancellationToken)
        {
            await botClient.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: text,
                        cancellationToken: cancellationToken
                        );
        }
        private async Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            Console.WriteLine(exception.Message);
        }
    }
}
