﻿using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using System.Threading.Tasks;
using FluentCommands.Attributes;
using FluentCommands.Menus;

namespace FluentCommands
{
    public class SampleCommandList
    {
        [Command("bap")]
        [Permissions(Permissions.Administrator | Permissions.CanSendMediaMessages)]
        public async Task Beyonce(TelegramBotClient client, UpdateEventArgs e)
        {
            await Console.Out.WriteLineAsync("testing...");
            await client.SendTextMessageAsync(e.Update.Message.From.Id, "woah!!!");
        }

        [Command("boop")]
        public async Task Googagaogaoagoao(TelegramBotClient client, MessageEventArgs e)
        {
            await Console.Out.WriteLineAsync("messagetest");
        }
    }
}