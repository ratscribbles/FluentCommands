using System;
using System.Collections.Generic;
using System.Text;
using FluentCommands.Attributes;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace FluentCommands
{
    public class MyCommands
    {
        [ModuleBuilder]
        public static void OnBuilding()
        {
            CommandService.Module<CommandList>(c =>
            {
                c["start"]
                    .HasAliases("one", "two", "three")
                    .HasHelpDescription("poggers").WithParseMode(ParseMode.Default);

                c["big test"].HasAliases("wow").HasKeyboardButton(new KeyboardButton());
                c["help"].HasHelpDescription("hehe").WithParseMode(ParseMode.Default);
                c["gig"].HasAliases("");
                c["keyboardtest"]
                    .HasAliases("keyboop")
                    .HasKeyboard(k =>
                    {
                        k.AddRow(new InlineKeyboardButton { Text = "lmfao", CallbackData = "summercat" }, k["summercat"]);
                    });
                c["summercat"].HasKeyboardButton(new InlineKeyboardButton { Text = "i'm summercat", CallbackData = "keyboardtest" });
            });
        }
    }
}
