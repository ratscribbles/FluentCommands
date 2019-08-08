using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using FluentCommands.Builders;
using Telegram.Bot.Types;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace FluentCommands
{
    internal enum KeyboardType { None, Inline, Reply }
    internal class Command
    {
        internal Type Module { get; private set; }
        internal string Name { get; private set; }
        internal string[] Aliases { get; private set; } = Array.Empty<string>();
        internal string Description { get; private set; } = "";
        internal ParseMode ParseMode { get; private set; } = ParseMode.Default;
        internal IKeyboardButton Button { get; private set; } = null;
        internal KeyboardType KeyboardType { get; private set; } = KeyboardType.None;
        internal ReplyKeyboardMarkup ReplyKeyboard { get; private set; } = null;
        internal InlineKeyboardMarkup InlineKeyboard { get; private set; } = null;

        private protected Command(CommandBase commandBase)
        {
            Module = commandBase.Module;
            Name = commandBase.Name;
            Aliases = commandBase.Aliases;
            Description = commandBase.Description;
            ParseMode = commandBase.ParseMode;
            Button = commandBase.Button;

            if(commandBase.KeyboardInfo != null)
            {
                if (commandBase.KeyboardInfo.Inline != null)
                {
                    InlineKeyboard = new InlineKeyboardMarkup(commandBase.KeyboardInfo.Inline.Rows);
                    KeyboardType = KeyboardType.Inline;
                }
                if (commandBase.KeyboardInfo.Reply != null)
                {
                    ReplyKeyboard = new ReplyKeyboardMarkup(commandBase.KeyboardInfo.Reply.Rows);
                    KeyboardType = KeyboardType.Reply;
                }
            }
        }
    }
}
