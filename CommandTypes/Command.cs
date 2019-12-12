using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using FluentCommands.Attributes;
using FluentCommands.Builders;
using Telegram.Bot.Types;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using FluentCommands.CommandTypes.Steps;
using Telegram.Bot;
using FluentCommands.Helper;

namespace FluentCommands.CommandTypes
{
    internal delegate Task CommandDelegate<TArgs>(TelegramBotClient c, TArgs e) where TArgs : EventArgs;
    internal delegate Task<TReturn> CommandDelegate<TArgs, TReturn>(TelegramBotClient c, TArgs e) where TArgs : EventArgs;
    internal enum CommandType { Default, ReplyKeyboard, Step }
    internal enum KeyboardType { None, Inline, Reply, ForceReply, Remove }
    internal sealed class Command<TArgs> : ICommand where TArgs : EventArgs
    {
        internal Type Module { get; }
        internal string Name { get; }
        internal CommandType CommandType { get; }
        internal string[] Aliases { get; } = Array.Empty<string>();
        internal string Description { get; } = string.Empty;
        internal ParseMode ParseMode { get; } = ParseMode.Default;
        internal IKeyboardButton? Button { get; } = null;
        internal IReplyMarkup? ReplyMarkup { get; } = null;
        internal KeyboardType KeyboardType { get; } = KeyboardType.None;
        internal CommandDelegate<TArgs> Invoke { get; }
        Type ICommand.Module => Module;
        Type ICommand.Args { get; } = typeof(TArgs);
        string ICommand.Name => Name;
        CommandType ICommand.CommandType => CommandType;

        internal Command(CommandBaseBuilder commandBase, MethodInfo method, Type module)
        {
            Module = module;
            Name = commandBase.Name;
            Aliases = commandBase.InAliases;
            Description = commandBase.InDescription;
            ParseMode = commandBase.InParseMode;
            Button = commandBase.InButton;
            CommandType = commandBase.CommandType;

            if (!(commandBase.KeyboardInfo is null))
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

            //* This is for new features, to help separate them from the original implementation. *//
            #region Extensibility Constructor
            Permissions = commandBase.Permissions;
            StepInfo = commandBase.StepInfo;
            #endregion

            if (CommandType != CommandType.Default)
            {
                // Primary invoker is not used.
                Invoke = (client, e) => Task.CompletedTask;
            }
            else
            {
                if (AuxiliaryMethods.TryConvertDelegate<TArgs>(method, out var c)) Invoke = c;
                else throw new ArgumentException();
            }
        }

        //* This is for new features, to help separate them from the original implementation. *//
        #region Extensibility Support
        //
        #region Properties
        internal Permissions Permissions { get; } = Permissions.None;
        internal StepContainer? StepInfo { get; }
        StepContainer? ICommand.StepInfo => StepInfo;
        #endregion
        //
        #endregion
    }
}
