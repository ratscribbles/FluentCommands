using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using FluentCommands.Attributes;
using FluentCommands.Builders;
using FluentCommands.CommandTypes;
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
        internal Permissions Permissions { get; set; } = Permissions.None;
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
                if (commandBase.KeyboardInfo.InlineKeyboard != null)
                {
                    InlineKeyboard = new InlineKeyboardMarkup(commandBase.KeyboardInfo.InlineKeyboard.Rows);
                    KeyboardType = KeyboardType.Inline;
                }
                if (commandBase.KeyboardInfo.ReplyKeyboard != null)
                {
                    ReplyKeyboard = new ReplyKeyboardMarkup(commandBase.KeyboardInfo.ReplyKeyboard.Rows);
                    KeyboardType = KeyboardType.Reply;
                }
            }
        }
    }
}
