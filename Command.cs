using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using FluentCommands.Attributes;
using FluentCommands.Builders;
using FluentCommands.CommandTypes;
using Telegram.Bot.Types;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace FluentCommands
{
    internal enum KeyboardType { None, Inline, Reply, ForceReply, Remove }
    internal class Command
    {
        internal Type Module { get; private set; }
        internal string Name { get; private set; }
        internal Permissions Permissions { get; set; } = Permissions.None;
        internal string[] Aliases { get; private set; } = Array.Empty<string>();
        internal string Description { get; private set; } = "";
        internal ParseMode ParseMode { get; private set; } = ParseMode.Default;
        internal IKeyboardButton? Button { get; private set; } = null;
        internal IReplyMarkup? ReplyMarkup { get; private set; } = null;
        internal KeyboardType KeyboardType { get; private set; } = KeyboardType.None;

        private protected Command(CommandBaseBuilder commandBase, Type module)
        {
            Module = module;
            Name = commandBase.Name;
            Aliases = commandBase.InAliases;
            Description = commandBase.InDescription;
            ParseMode = commandBase.InParseMode;
            Button = commandBase.InButton;

            if(!(commandBase.KeyboardInfo is null))
            {
                switch (commandBase.KeyboardInfo)
                {
                    case var _ when commandBase.KeyboardInfo.InlineRows.Any():
                        ReplyMarkup = new InlineKeyboardMarkup(commandBase.KeyboardInfo.InlineRows);
                        KeyboardType = KeyboardType.Inline;
                        break;
                    case var _ when commandBase.KeyboardInfo.ReplyRows.Any():
                        ReplyMarkup = new ReplyKeyboardMarkup(commandBase.KeyboardInfo.ReplyRows) 
                        { 
                            OneTimeKeyboard = commandBase.KeyboardInfo.OneTimeKeyboard,
                            ResizeKeyboard = commandBase.KeyboardInfo.ResizeKeyboard,
                            Selective = commandBase.KeyboardInfo.Selective
                        };
                        KeyboardType = KeyboardType.Reply;
                        break;
                    case var _ when !(commandBase.KeyboardInfo.ForceReply is null):
                        ReplyMarkup = new ForceReplyMarkup { Selective = commandBase.KeyboardInfo.Selective };
                        KeyboardType = KeyboardType.ForceReply;
                        break;
                    case var _ when !(commandBase.KeyboardInfo.ReplyRemove is null):
                        ReplyMarkup = new ReplyKeyboardRemove() { Selective = commandBase.KeyboardInfo.Selective };
                        KeyboardType = KeyboardType.Remove;
                        break;
                    default:
                        ReplyMarkup = null;
                        break;
                }
            }
        }
    }
}
