using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FluentCommands.Attributes;
using FluentCommands.Menus;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace FluentCommands.Tests.Integration
{
    public class TestModule2 : CommandModule<TestModule2>
    {
        protected override void OnConfiguring(ModuleConfigBuilder config)
        {
            config.AddClient(Program.Client3);
        }

        [Command("test")]
        public async Task Test(TelegramBotClient client, MessageEventArgs e)
        {
        }

        [Command("thinking")]
        public async Task Wow(MessageContext c)
        {
        }
    }

    public class TestModule : CommandModule<TestModule>
    {
        protected override void OnConfiguring(ModuleConfigBuilder config)
        {
            config.AddClient(Program.Client2);
            config.DeleteCommandAfterCall = true;
        }

        [Command("test")]
        public async Task Test(TelegramBotClient client, MessageEventArgs e)
        {
            await Menu.Text($"Ok! My botId is {client.BotId}.").Send(client, e);
        }
    }
}
