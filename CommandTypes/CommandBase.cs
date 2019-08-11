using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using FluentCommands.Builders;

namespace FluentCommands.CommandTypes
{
    internal class CommandBase
    {
        internal Type Module { get; set; }
        internal string Name { get; set; }
        internal string[] Aliases { get; set; } = Array.Empty<string>();
        internal string Description { get; set; } = "There is no description for this command.";
        internal ParseMode ParseMode { get; set; } = ParseMode.Default;
        internal IKeyboardButton Button { get; set; } = null;
        internal KeyboardBuilder KeyboardInfo { get; set; } = null;

        internal CommandBase() { }
        internal CommandBase(string name) => Name = name;
    }
}
