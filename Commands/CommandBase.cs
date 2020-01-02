using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Telegram.Bot.Types;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using FluentCommands.Commands.Steps;
using Telegram.Bot;
using FluentCommands.Utility;
using FluentCommands.Menus;
using FluentCommands.Interfaces.MenuBuilders;

namespace FluentCommands.Commands
{
    internal delegate Task CommandDelegate<TContext, TArgs>(TContext e) where TContext : ICommandContext<TArgs> where TArgs : EventArgs;
    internal delegate Task<TReturn> CommandDelegate<TContext, TArgs, TReturn>(TContext e) where TContext : ICommandContext<TArgs> where TArgs : EventArgs;
    internal class CommandBase<TContext, TArgs> : ICommand where TContext : ICommandContext<TArgs> where TArgs : EventArgs
    {
        private readonly ISendableMenu? _errorMsg;
        internal Type Module { get; }
        internal string Name { get; }
        internal CommandType CommandType { get; }
        internal Permissions Permissions { get; } = Permissions.None;
        internal string[] Aliases { get; } = Array.Empty<string>();
        internal ISendableMenu Description { get; } = Menu.Text("There is no description for this command.");
        internal ISendableMenu ErrorMsg 
        {
            get
            {
                if (_errorMsg is null) return CommandService.Modules[Module].Config.DefaultErrorMessageOverride;
                else return _errorMsg;
            }
        }
        internal InlineKeyboardButton? Button { get; } = null;
        internal CommandDelegate<TContext, TArgs> Invoke { get; }
        Type ICommand.Module => Module;
        Type ICommand.Context { get; } = typeof(TContext);
        string ICommand.Name => Name;
        string[] ICommand.Aliases => Aliases;
        CommandType ICommand.CommandType => CommandType;
        Permissions ICommand.Permissions => Permissions;
        ISendableMenu ICommand.Description => Description;
        ISendableMenu ICommand.ErrorMsg => ErrorMsg;
        InlineKeyboardButton? ICommand.Button => Button;

        internal CommandBase(CommandBaseBuilder commandBase, MethodInfo method, Type module)
        {
            Module = module;
            Name = commandBase.Name;
            Aliases = commandBase.InAliases;
            Permissions = commandBase.Permissions;
            Description = commandBase.InHelpDescription;
            _errorMsg = commandBase.ErrorMsg;
            Button = commandBase.InButton;
            CommandType = commandBase.CommandType;

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
