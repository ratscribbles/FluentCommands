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

        protected override void OnConfiguring(ModuleBuilderConfig moduleBuilderConfig)
        {
            moduleBuilderConfig.LogModuleActivities = false;
            moduleBuilderConfig.UseLoggingEventHandler = OnLogging;
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
    }
}
