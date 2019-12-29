using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FluentCommands.Commands;
using FluentCommands.Menus;

namespace FluentCommands.Tests.Integration
{
    public class TestModule
    {
        [Command("test")]
        public async Task lmao(MessageContext ctx)
        {
            // shouldnt work
            await Menu.Text("poggers").Send(ctx);
            await ctx.Client.SendTextMessageAsync(ctx.EventArgs.Message.Chat.Id, "uh oh");
        }
    }
}
