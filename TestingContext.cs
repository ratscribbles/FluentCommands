using System;
using System.Collections.Generic;
using System.Text;
using FluentCommands.Builders;
using FluentCommands.Attributes;
using Telegram.Bot.Types.ReplyMarkups;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using FluentCommands.Logging;
using FluentCommands.Menus;
using FluentCommands.CommandTypes.Steps;

namespace FluentCommands
{
    public class TestingContext : CommandModule<TestingContext>
    {
        //protected override void OnConfiguring(ModuleConfigBuilder config)
        //{
        //    config.LogModuleActivities = true;
        //    config.UseLoggingEventHandler = OnLogging;
        //}

        //public static async Task OnLogging(FluentLoggingEventArgs args)
        //{
        //    await Console.Out.WriteLineAsync("RECEIVED FROM THE MODULE CONTEXT!! POGGERS");
        //}



        //[Command("e")]
        //[Step(5)]
        //public async Task<IStep> B(TelegramBotClient client, MessageEventArgs e)
        //{
        //    await client.SendTextMessageAsync(e.Message.Chat.Id, "test successful");
        //    return Step.Success();

        //}

        [Command("e")]
        public async Task PwOOP(TelegramBotClient client, MessageEventArgs e)
        {
            await client.SendTextMessageAsync(e.Message.Chat.Id, "test successful");
        }

        [Command("pee")]
        public async Task Wow(TelegramBotClient client, MessageEventArgs e)
        {
            await client.SendTextMessageAsync(e.Message.Chat.Id, "POGGERS");
            await client.SendTextMessageAsync(e.Message.Chat.Id, "POGGERS");
            await client.SendTextMessageAsync(e.Message.Chat.Id, "POGGERS");
            await client.SendTextMessageAsync(e.Message.Chat.Id, "POGGERS");
        }
        //[Command("e")]
        //[Step(5)]
        //public async Task<IStep> C(TelegramBotClient client, MessageEventArgs e)
        //{
        //    await client.SendTextMessageAsync(e.Message.Chat.Id, "test successful");
        //    return Step.Success();

        //}

        //[Command("e")]
        //[Step(4)]
        //public async Task<IStep> D(TelegramBotClient client, MessageEventArgs e)
        //{
        //    await client.SendTextMessageAsync(e.Message.Chat.Id, "test successful");
        //    return Step.Success();

        //}

        //[Command("e")]
        //[Step(5)]
        //public async Task<IStep> F(TelegramBotClient client, MessageEventArgs e)
        //{
        //    await client.SendTextMessageAsync(e.Message.Chat.Id, "test successful");
        //    return Step.Success();
        //}

    }
}
