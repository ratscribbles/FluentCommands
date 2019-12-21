using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FluentCommands;
using FluentCommands.Attributes;
using FluentCommands.Menus;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace FluentCommands.Tests.Integration
{
    public class TestModule : CommandModule<TestModule>
    {
        [Command("test")]
        public async Task Test(TelegramBotClient client, MessageEventArgs e)
        {
            await Menu.Text($"Ok! My botId is {client.BotId}.").Send(client, e);
        }
    }
}
