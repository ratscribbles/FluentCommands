using System;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Args;
using System.Threading.Tasks;
using FluentCommands.Menus;

namespace FluentCommands
{
    class Program
    {
        public static TelegramBotClient Client = new TelegramBotClient("879319589:AAFnT8ROf-QokfusSSUFLgMG_VJxUz8jNks");
        public static Message msg = null;

        static async Task Main(string[] args)
        {
            Console.Clear();

            CommandService.Start();

            Client.OnUpdate += Bot_OnUpdate;
            Client.OnMessage += Bot_OnMessage;

            Client.StartReceiving(Array.Empty<UpdateType>());

            Console.WriteLine();

            Console.ReadLine();
            Client.StopReceiving();
        }

        static async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            MenuItem.As().Text().TextSource("").Done();
            if(e.Message.Text == "joj")
            {
                await Client.SendTextMessageAsync(e.Message.Chat.Id, "BIG TEST");
                await Client.SendTextMessageAsync(e.Message.Chat.Id, "—", replyMarkup: new InlineKeyboardMarkup(new InlineKeyboardButton[] { InlineKeyboardButton.WithCallbackData("Lmao"), InlineKeyboardButton.WithCallbackData("Lmao") }));
            }

            //await Client.SendTextMessageAsync(e.Message.Chat.Id, "bujabbers", replyMarkup: new InlineKeyboardMarkup(new[] { InlineKeyboardButton.WithCallbackData("lmfao") }));
            //var keyboard = new ReplyKeyboardMarkup(new KeyboardButton[] { new KeyboardButton("wow"), new KeyboardButton("big pog") });
            //keyboard.OneTimeKeyboard = true;
            //var msg = await Client.SendTextMessageAsync(e.Message.Chat.Id, "testing...", replyMarkup: keyboard);
            //await Client.EditMessageReplyMarkupAsync(msg.Chat.Id, msg.MessageId, InlineKeyboardMarkup.Empty());
            //await CommandService.Evaluate<CommandList>(Client, e);
        }

        static async void Bot_OnUpdate(object sender, UpdateEventArgs e)
        {
            // await Client.AnswerCallbackQueryAsync(e.Update.CallbackQuery.Id, "LMFAO testing");

            //await Client.DeleteMessageAsync(e.Update.Message.Chat.Id, e.Update.Message.MessageId);


            //if (msg == null)
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
