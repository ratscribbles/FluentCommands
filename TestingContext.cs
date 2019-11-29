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
        protected override void OnBuilding(ModuleBuilder m)
        {
            m.Command("e")
                .Aliases("lmao", "lmao", "lmao", "lmao", "lmao", "lmao", "lmao", "lmao", "lmao", "lmao", "lmao", "lmao", "lmao", "lmao", "lmao", "lmao")
                .HelpDescription("lmao")
                .UsingParseMode(Telegram.Bot.Types.Enums.ParseMode.Default)
                .ReplyMarkup().Inline(k => { });
        }

        protected override void OnConfiguring(ModuleConfigBuilder config)
        {
            config.LogModuleActivities = true;
            config.UseLoggingEventHandler = OnLogging;
        }

        public static async Task OnLogging(FluentLoggingEventArgs args)
        {
            await Console.Out.WriteLineAsync("RECEIVED FROM THE MODULE CONTEXT!! POGGERS");
        }

        [Command("e")]
        public async Task E(TelegramBotClient client, MessageEventArgs e)
        {
            await client.SendTextMessageAsync(e.Message.Chat.Id, "test successful");
        }

        public async Task<(Menu, ChainResult<int>)> woweee(TelegramBotClient client, MessageEventArgs e)
        {
            return (MenuItem.As().Animation().Source("").Done(), 0);
        }

        [Command("e")]
        [Step(-1)]
        public async Task<IStep> ppepepe(TelegramBotClient client, MessageEventArgs e)
        {
            await client.SendTextMessageAsync(e.Message.Chat.Id, "test successful");
        }
    }
}
