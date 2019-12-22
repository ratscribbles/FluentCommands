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
using FluentCommands.CommandTypes.Steps;
using FluentCommands.Interfaces.KeyboardBuilders;
using FluentCommands.Builders;
using FluentCommands.Logging;
using FluentCommands.CommandTypes;
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

        public static Message? msg = null;

        static async Task Main(string[] args)
        {
            CommandService.Start(c =>
            {
                c.Logging = true;
                c.MaximumLogLevel = FluentLogLevel.Debug;
                c.CaptureAllLoggingEvents = true;
                c.AddClient(Client);
                c.UseDefaultErrorMsg = true;
            });

            //Client.OnUpdate += Bot_OnUpdate;
            //Client.OnMessage += Bot_OnMessage;
            //Client.OnInlineQuery += Bot_OnInline;
            //Client.OnInlineResultChosen += Bot_OnChosen;

            //Client.StartReceiving(Array.Empty<UpdateType>());
            //Client2.StartReceiving(Array.Empty<UpdateType>());
            //Client3.StartReceiving(Array.Empty<UpdateType>());

            Console.WriteLine();
            Console.ReadLine();
            //Client.StopReceiving();
        }

        static async void Bot_OnMessage(object? sender, MessageEventArgs e)
        {


            if (e.Message.Text == "joj")
            {
                await Client.SendTextMessageAsync(e.Message.Chat.Id, "BIG TEST");
                await Client.SendTextMessageAsync(e.Message.Chat.Id, "—", replyMarkup: new InlineKeyboardMarkup(new InlineKeyboardButton[] { InlineKeyboardButton.WithCallbackData("Lmao"), InlineKeyboardButton.WithCallbackData("Lmao") }));
            }

            if (e.Message.Text == "lmao")
            {
                //: explore multiple client support...
                //: explore logo for the project (probably a paper airplane of some kind, but like, with a gear or something? just fuck around
                //: OK




                //! FluentCommands, a framework for TelegramBots.Net


                //: in the docs, talk about (and compare) th anatomy of a Menu vs a raw client.sendtext request. discuss the structure, as well as the reduction of superfluous boilerplate
                await Menu.Text("Hi").Send(Client, e);
            }

            if (e.Message.Text == "lmao1")
            {
                await Menu.Text("Edited 1").Send(Client, e);
            }

            if (e.Message.Text == "lmao2")
            {
                await Menu.Text("Edited 2").Send(Client, e);
            }

            if (e.Message.Text == "testaction")
            {
                await Menu.Text("actiontested").Send(Client, e, ChatAction.UploadAudio, 3000);
            }
        }

        static async void Bot_OnInline(object? sender, InlineQueryEventArgs e)
        {
            if (e.InlineQuery.Query == "lmao")
            {
                await Menu.Text("Does this work?").Send(Client, e);
            }

            //: create query simplifier (like menus). go whole hog on this fucking project my dude
            if (e.InlineQuery.Query == "hmm")
            {
                await Client.AnswerInlineQueryAsync(e.InlineQuery.Id, new[] { new InlineQueryResultArticle("huh", "lmao", new InputTextMessageContent("CONTENT")) });
            }
        }

        static async void Bot_OnChosen(object? sender, ChosenInlineResultEventArgs e)
        {
            await Console.Out.WriteLineAsync("event received");
            await Menu.Text("this is pretty neat").Send(Client, e);
            await Client.SendTextMessageAsync(e.ChosenInlineResult.From.Id, "testing");
        }

        static async void Bot_OnUpdate(object? sender, UpdateEventArgs e)
        {
            await Console.Out.WriteLineAsync("event received");
            // await Client.AnswerCallbackQueryAsync(e.Update.CallbackQuery.Id, "LMFAO testing");

            //await Client.DeleteMessageAsync(e.Update.Message.Chat.Id, e.Update.Message.MessageId);


            //if (msg is null)
            //{
            //    msg = await Client.SendTextMessageAsync(e.Update.Message?.Chat.Id ?? e.Update.CallbackQuery.From.Id, "GIGA");
            //    await Client.EditMessageReplyMarkupAsync(msg.Chat.Id, msg.MessageId, new InlineKey    oardMarkup(InlineKeyboardButton.WithCallbackData("PEE")));
            //}
            //else
            //{
            //    try
            //    {
            //        await Client.EditMessageTextAsync(msg.Chat.Id, msg.MessageId, "MONKA", replyMarkup: new InlineKeyboardMarkup(InlineKeyboardButton.WithCallbackData("GIGA")));

            //    }
            //    catch(MessageIsNotModifiedException)
            //    {
            //        try
            //        {
            //            await Client.EditMessageTextAsync(msg.Chat.Id, msg.MessageId, "GIGA", replyMarkup: new InlineKeyboardMarkup(InlineKeyboardButton.WithCallbackData("PEE")));
            //        }
            //        catch(MessageIsNotModifiedException)
            //        {
            //            await Client.EditMessageTextAsync(msg.Chat.Id, msg.MessageId, "MONKA", replyMarkup: new InlineKeyboardMarkup(InlineKeyboardButton.WithCallbackData("GIGA")));
            //        }
            //    }

            //    //await Client.DeleteMessageAsync(msg.Chat.Id, msg.MessageId);
            //    //msg = await Client.SendTextMessageAsync(msg.Chat.Id, "GIGA");
            //    //await Client.EditMessageReplyMarkupAsync(msg.Chat.Id, msg.MessageId, new InlineKeyboardMarkup(InlineKeyboardButton.WithCallbackData("PEE")));
            //}
        }
    }
}
