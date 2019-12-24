using System;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Args;
using System.Threading.Tasks;
using FluentCommands;
using FluentCommands.Menus;
using FluentCommands.Interfaces.KeyboardBuilders;
using FluentCommands.Builders;
using FluentCommands.Logging;
using Telegram.Bot.Types.InlineQueryResults;
using System.Text.RegularExpressions;
using System.Linq;

namespace FluentCommands.Tests.Integration
{
    class Program
    {
        private static readonly string _token = System.IO.File.ReadLines(@"E:\Dropbox\FluentCommands\botinf1.txt").ElementAt(0);
        private static readonly string _token2 = System.IO.File.ReadLines(@"E:\Dropbox\FluentCommands\botinf2.txt").ElementAt(0);
        private static readonly string _token3 = System.IO.File.ReadLines(@"E:\Dropbox\FluentCommands\botinf3.txt").ElementAt(0);
        public static readonly TelegramBotClient Client = new TelegramBotClient(_token);
        public static readonly TelegramBotClient Client2 = new TelegramBotClient(_token2);
        public static readonly TelegramBotClient Client3 = new TelegramBotClient(_token3);
        public static readonly int Client1_Id = int.Parse(System.IO.File.ReadLines(@"E:\Dropbox\FluentCommands\botinf1.txt").ElementAt(1));
        public static readonly int Client2_Id = int.Parse(System.IO.File.ReadLines(@"E:\Dropbox\FluentCommands\botinf2.txt").ElementAt(1));
        public static readonly int Client3_Id = int.Parse(System.IO.File.ReadLines(@"E:\Dropbox\FluentCommands\botinf3.txt").ElementAt(1));

        static void Main(string[] args)
        {
            CommandService.Start(crfg =>
            {
                crfg.AddClient(Client);
                crfg.Logging = true;
                crfg.MaximumLogLevel = FluentLogLevel.Debug;
                crfg.CaptureAllLoggingEvents = true;
                crfg.UseDefaultErrorMsg = true;
            });

            Console.ReadLine();
        }
    }
}
