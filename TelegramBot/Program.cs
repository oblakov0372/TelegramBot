using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace TelegramBot
{
    internal class Program
    {
        private const string KEY = "5952421799:AAErUecCxkX-4B3Tw1oRSI8W_28T7r9R5ho";
        static async Task Main(string[] args)
        {
            var botClient = new TelegramBotClient(KEY);

            var metBot = new BotEngine(botClient);

            await metBot.ListenForMessagesAsync();
        }
    }
}
