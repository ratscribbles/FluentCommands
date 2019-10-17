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
    public class SampleTestModule
    {
        [ModuleBuilder]
        public static void OnBuilding()
        {
            CommandService.Module<SampleCommandList>(c =>
            {
                c["start"]
                    .HasAliases("one", "two", "three")
                    .HasHelpDescription("help!!")
                    .WithParseMode(ParseMode.Default);
                c["big test"]
                    .HasAliases("wow")
                    .HasKeyboardButton(new KeyboardButton());
                c["help"]
                    .HasHelpDescription("h")
                    .WithParseMode(ParseMode.Default);
                c["keyboardtest"]
                    .HasAliases("keyboop")
                    .HasKeyboard(k =>
                    {
                        k.AddRow(new InlineKeyboardButton { Text = "lmfao", CallbackData = "callback" }, k["key"]);
                        //: Refactor to handle ambiguous buttons in row; roslyn to check if incompatible buttons
                    });
                c["solo"]
                    .HasKeyboardButton(new InlineKeyboardButton { Text = "solobuttontest", CallbackData = "callbacc" });
            });
        }
    }
}
