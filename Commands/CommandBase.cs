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
using FluentCommands.Commands.Steps;
using Telegram.Bot;
using FluentCommands.Utility;
using FluentCommands.Menus;

namespace FluentCommands.Commands
{
    internal delegate Task CommandDelegate<TContext, TArgs>(TContext e) where TContext : ICommandContext<TArgs> where TArgs : EventArgs;
    internal delegate Task<TReturn> CommandDelegate<TContext, TArgs, TReturn>(TContext e) where TContext : ICommandContext<TArgs> where TArgs : EventArgs;
    internal sealed class CommandBase<TContext, TArgs> : ICommand where TContext : ICommandContext<TArgs> where TArgs : EventArgs
    {
        internal Type Module { get; }
        internal string Name { get; }
        internal CommandType CommandType { get; }
        internal string[] Aliases { get; } = Array.Empty<string>();
        internal string Description { get; } = string.Empty;
        internal ParseMode ParseMode { get; } = ParseMode.Default;
        internal IKeyboardButton? Button { get; } = null;
        internal CommandDelegate<TContext, TArgs> Invoke { get; }
        Type ICommand.Module => Module;
        Type ICommand.Args { get; } = typeof(TArgs);
        string ICommand.Name => Name;
        CommandType ICommand.CommandType => CommandType;

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

        internal CommandBase(CommandBaseBuilder commandBase, MethodInfo method, Type module)
        {
            Module = module;
            Name = commandBase.Name;
            Aliases = commandBase.InAliases;
            Description = commandBase.InDescription;
            ParseMode = commandBase.InParseMode;
            Button = commandBase.InButton;
            CommandType = commandBase.CommandType;

            //* This is for new features, to help separate them from the original implementation. *//
            #region Extensibility Constructor
            Permissions = commandBase.Permissions;
            StepInfo = commandBase.StepInfo;
            #endregion

            if (CommandType != CommandType.Default)
            {
                // Primary invoker is not used.
                Invoke = (ctx) => Task.CompletedTask;
            }
            else
            {
                if (AuxiliaryMethods.TryConvertDelegate<TContext, TArgs>(method, out var c)) Invoke = c;
                else throw new ArgumentException();
            }
        }
    }
}
