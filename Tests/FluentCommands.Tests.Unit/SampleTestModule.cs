using System;
using System.Collections.Generic;
using System.Text;
using FluentCommands.Builders;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace FluentCommands
{
    public class SampleTestModule : CommandContext<SampleCommandList>
    {
        protected override void OnBuilding(ModuleBuilder moduleBuilder)
        {
            //: Redo documentation to describe what each step is actually adding to the objects
            //: consider removing the ambiguity with concrete implementations (requires editing hte interfaces; it shouldnt be too big a deal)
            moduleBuilder.Command("").HasAliases("").HasKeyboard("")


            moduleBuilder["start"]
                .HasAliases("one", "two", "three")
                .HasHelpDescription("help!!")
                .WithParseMode(ParseMode.Default);
            moduleBuilder["big test"]
                .HasAliases("wow")
                .HasKeyboardButton(new KeyboardButton());
            moduleBuilder["help"]
                .HasHelpDescription("h")
                .WithParseMode(ParseMode.Default);
            moduleBuilder["keyboardtest"]
                .HasAliases("keyboop")
                .HasKeyboard().Inline(k =>
                {
                    k.AddRow(
                        new InlineKeyboardButton { Text = "lmfao", CallbackData = "callback" },
                        k["key"].InModule<SampleTestModule>(),
                        k["key"],
                        k["key"].InModule(typeof(SampleTestModule)));
                });
            moduleBuilder["solo"]
                .HasKeyboardButton(new InlineKeyboardButton { Text = "solobuttontest", CallbackData = "callbacc" });
            moduleBuilder["testingFluentSyntax"]
                .HasAliases("euahdsoidhsaoihasoidsaoidaisod")
                .HasHelpDescription("i need help lmao")
                .WithParseMode(ParseMode.Default)
                .HasKeyboard().Reply(k =>
                {
                    //: Roslyn analyze when user inputs the wrong button type for this method; suggest they either swap methods or check their buttons

                    //? k.AddRow(InlineKeyboardButton.WithCallbackData(""));
                    k.AddRow();
                    k.AddRow();
                    k.AddRow();
                })
                .HasKeyboardButton(InlineKeyboardButton.WithCallbackData("AHHHHHHHH"));
        }
    }
}
