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
        [Command("e")]
        [Step(0)]
        public async Task PwOOP(TelegramBotClient client, MessageEventArgs e)
        {
            await client.SendTextMessageAsync(e.Message.Chat.Id, "test successful");
        }
        [Command("e")]
        [Step(1)]
        public async Task<IStep> PweOOP(TelegramBotClient client, MessageEventArgs e)
        {
            await client.SendTextMessageAsync(e.Message.Chat.Id, "1 test successful");
            return Step.Success();
        }
        [Command("e")]
        [Step(2)]
        public async Task<IStep> PwOaOP(TelegramBotClient client, MessageEventArgs e)
        {
            await client.SendTextMessageAsync(e.Message.Chat.Id, "2 test successful");
            return Step.Success();
        }
        [Command("e")]
        [Step(3)]
        public async Task<IStep> PwfOOP(TelegramBotClient client, MessageEventArgs e)
        {
            await client.SendTextMessageAsync(e.Message.Chat.Id, "3 test successful");
            return Step.Success();
        }

        [Command("pee")]
        public async Task Wow(TelegramBotClient client, MessageEventArgs e)
        {
            await client.SendTextMessageAsync(e.Message.Chat.Id, "POGGERS");
            await client.SendTextMessageAsync(e.Message.Chat.Id, "POGGERS");
            await client.SendTextMessageAsync(e.Message.Chat.Id, "POGGERS");
            await client.SendTextMessageAsync(e.Message.Chat.Id, "POGGERS");
        }
    }
}
