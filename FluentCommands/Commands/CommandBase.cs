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
using FluentCommands.Interfaces.BuilderBehaviors.ModuleBuilderBehaviors;

namespace FluentCommands.Commands
{
    internal abstract class CommandBase : ICommand 
    {
        private readonly ISendableMenu? _helpMsg;
        private readonly ISendableMenu? _errorMsg;
        internal virtual CommandType CommandType { get; }
        internal Type Module { get; }
        internal string Name { get; }
        internal Permissions Permissions { get; } = Permissions.None;
        internal string[] Aliases { get; } = Array.Empty<string>();
        internal ISendableMenu Description
        {
            get
            {
                if (_helpMsg is null) return Menu.Text("There is no description for this command.");
                else return _helpMsg;
            }
        }
        internal ISendableMenu ErrorMsg 
        {
            get
            {
                if (_errorMsg is null) return CommandService.Modules[Module].Config.DefaultErrorMessageOverride;
                else return _errorMsg;
            }
        }
        internal InlineKeyboardButton? Button { get; } = null;
        Type ICommand.Module => Module;
        string ICommand.Name => Name;
        string[] ICommand.Aliases => Aliases;
        CommandType ICommand.CommandType => CommandType;
        Permissions ICommand.Permissions => Permissions;
        ISendableMenu ICommand.Description => Description;
        ISendableMenu ICommand.ErrorMsg => ErrorMsg;
        InlineKeyboardButton? ICommand.Button => Button;

        internal CommandBase(CommandBaseBuilder commandBase, Type module)
        {
            Module = module;
            Name = commandBase.Name;
            Aliases = commandBase.Aliases;
            Permissions = commandBase.Permissions;
            _helpMsg = commandBase.HelpDescription;
            _errorMsg = commandBase.ErrorMessage;
            Button = commandBase.Button;
        }
    }
}
