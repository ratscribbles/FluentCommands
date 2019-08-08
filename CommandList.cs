using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using System.Threading.Tasks;
using FluentCommands.Attributes;
using FluentCommands.Menu;

namespace FluentCommands
{
    public class CommandList
    {
        [Command("bap")]
        public async Task Beyonce(TelegramBotClient client, UpdateEventArgs e)
        {
            await Console.Out.WriteLineAsync("PEE");
            await client.SendTextMessageAsync(e.Update.Message.From.Id, "BUJABBERS");
        }

        [Command("sus")]
        public async Task Googagaogaoagoao(TelegramBotClient client, MessageEventArgs e)
        {
            await Console.Out.WriteLineAsync("PEE");
        }
    }
}
