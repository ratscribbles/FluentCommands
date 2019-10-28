using System;
using System.Collections.Generic;
using System.Text;
using FluentCommands.Builders;
using FluentCommands.Attributes;
using Telegram.Bot.Types.ReplyMarkups;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;

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

        [Command("e")]
        public async Task E(TelegramBotClient client, MessageEventArgs e)
        {
            await client.SendTextMessageAsync(e.Message.Chat.Id, "test successful");
        }
    }
}
