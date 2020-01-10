using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using FluentCommands.Exceptions;
using FluentCommands.Interfaces;
using FluentCommands.Interfaces.BaseBuilders;
using FluentCommands.Interfaces.KeyboardBuilders;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using FluentCommands.Commands.Steps;
using System.Reflection;
using FluentCommands.Menus;
using FluentCommands.Interfaces.MenuBuilders;
using FluentCommands.Interfaces.BuilderBehaviors.ModuleBuilderBehaviors;

namespace FluentCommands.Commands
{
    /// <summary>
    /// Builder responsible for creating <see cref="Command"/> objects for the <see cref="CommandService"/>.
    /// </summary>
    public sealed class CommandBaseBuilder : ICommandBaseBuilder, ICommandBaseOnBuilding,
        ICommandBaseAliases, ICommandBaseErrorMessage, ICommandBaseDescription, IBuildInlineKeyboardButtonReference,
        IFluentInterface
    {
        internal CommandType CommandType { get; private set; }
        /// <summary>Gets the name of this <see cref="Command"/>.</summary>
        internal string Name { get; private set; }
        internal Permissions Permissions { get; private set; }
        internal StepContainer? StepInfo { get; private set; }
        internal string[] Aliases => (this as ICommandBaseBuilder).Out_Aliases;
        internal ISendableMenu? ErrorMessage => (this as ICommandBaseBuilder).Out_ErrorMessage;
        internal ISendableMenu? HelpDescription => (this as ICommandBaseBuilder).Out_HelpDescription;
        internal InlineKeyboardButton? Button => (this as ICommandBaseBuilder).Out_Button;

        #region Explicit Interface Properties
        string[] ICommandBaseBuilder.Out_Aliases => (this as IBuildAliases<ICommandBaseAliases>).In_Aliases;
        ISendableMenu? ICommandBaseBuilder.Out_HelpDescription => (this as IBuildErrorMessage<ICommandBaseErrorMessage>).In_ErrorMessage;
        ISendableMenu? ICommandBaseBuilder.Out_ErrorMessage => (this as IBuildHelpDescription<ICommandBaseDescription>).In_HelpDescription;
        InlineKeyboardButton? ICommandBaseBuilder.Out_Button => (this as IBuildInlineKeyboardButtonReference).In_Button;
        string[] IBuildAliases<ICommandBaseAliases>.In_Aliases { get; set; } = Array.Empty<string>();
        ISendableMenu? IBuildErrorMessage<ICommandBaseErrorMessage>.In_ErrorMessage { get; set; }
        ISendableMenu? IBuildHelpDescription<ICommandBaseDescription>.In_HelpDescription { get; set; }
        InlineKeyboardButton? IBuildInlineKeyboardButtonReference.In_Button { get; set; }
        #endregion

        /// <summary>
        /// Instantiates a new <see cref="CommandBaseBuilder"/>, which will be used to construct a <see cref="Command"/> for this Module.
        /// </summary>
        /// <param name="name">The name of this future <see cref="Command"/>.</param>
        internal CommandBaseBuilder(string name) => Name = name;

        //* This is for new features, to help separate them from the original implementation. *//
        internal void Set_CommandType(CommandType type) => CommandType = type;
        internal void Set_Permissions(PermissionsAttribute? p) => Permissions = p?.Permissions ?? Permissions.None;
    }
}
