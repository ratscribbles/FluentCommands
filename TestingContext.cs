using System;
using System.Collections.Generic;
using System.Text;
using FluentCommands.Builders;
using Telegram.Bot.Types.ReplyMarkups;

namespace FluentCommands
{
    public class TestingContext : CommandModule<TestingContext>
    {
        protected override void OnBuilding(ModuleBuilder m)
        {
            m["hi"]
                .Aliases("oweoweow", "djidaijsd")
                .HelpDescription("")
                .UsingParseMode(Telegram.Bot.Types.Enums.ParseMode.Default);
            m["poggers"]
                .HelpDescription("UEUEUEUE");
            m.Command("e")
                .Aliases("lmao", "lmao", "lmao", "lmao", "lmao", "lmao", "lmao", "lmao", "lmao", "lmao", "lmao", "lmao", "lmao", "lmao", "lmao", "lmao")
                .HelpDescription("lmao")
                .UsingParseMode(Telegram.Bot.Types.Enums.ParseMode.Default)
                .Keyboard().Inline(k => { });

        }
    }
}
