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

namespace FluentCommands.Commands
{
    /// <summary>
    /// Builder responsible for creating <see cref="Command"/> objects for the <see cref="CommandService"/>.
    /// </summary>
    public sealed class CommandBaseBuilder : ICommandBaseBuilder,
        ICommandBaseAliases, ICommandBaseDescription, //? Add as needed...
        IFluentInterface
    {
        internal CommandType CommandType { get; private set; }
        /// <summary>Gets the name of this <see cref="Command"/>.</summary>
        internal string Name { get; private set; }
        /// <summary>Gets the Aliases (other names) for this <see cref="Command"/>.</summary>
        internal string[] InAliases { get; private set; } = Array.Empty<string>();
        /// <summary>Gets the Description for this <see cref="Command"/>.</summary>
        internal ISendableMenu InHelpDescription { get; private set; } = Menu.Text("There is no description for this command.");
        /// <summary>Gets the <see cref="InlineKeyboardButton"/> for this <see cref="Command"/>. Used to call this command via Keyboard Markups, such as <see cref="InlineKeyboardMarkup"/> and <see cref="ReplyKeyboardMarkup"/>).</summary>
        internal InlineKeyboardButton? InButton { get; private set; } = null;

        /// <summary>
        /// Instantiates a new <see cref="CommandBaseBuilder"/>, which will be used to construct a <see cref="Command"/> for this Module.
        /// </summary>
        /// <param name="name">The name of this future <see cref="Command"/>.</param>
        internal CommandBaseBuilder(string name) => Name = name;

        #region Fluent Building
        /// <summary>
        /// Adds aliases (alternate names) to this <see cref="CommandBaseBuilder"/>. Duplicates are ignored.
        /// </summary>
        /// <param name="aliases">The alternate names for this future <see cref="Command"/>.</param>
        /// <returns>Returns this <see cref="CommandBaseBuilder"/> as an <see cref="ICommandBaseAliases"/>, removing this option from the fluent builder.</returns>
        public ICommandBaseAliases Aliases(params string[] aliases)
        {
            InAliases = aliases.Distinct().ToArray();
            return this;
        }
        /// <summary>
        /// Adds a description to this <see cref="CommandBaseBuilder"/>.
        /// </summary>
        /// <param name="description">The description of this future <see cref="Command"/>.</param>
        /// <returns>Returns this <see cref="CommandBaseBuilder"/> as an <see cref="ICommandBaseDescription"/>, prompting the user for a <see cref="Telegram.Bot.Types.Enums.ParseMode"/>.</returns>
        public ICommandBaseDescription HelpDescription(string description, ParseMode parseMode = ParseMode.Default)
        {
            InHelpDescription = Menu.Text(description).ParseMode(parseMode).Done();
            return this;
        }
        /// <summary>
        /// Adds a description to this <see cref="CommandBaseBuilder"/>.
        /// </summary>
        /// <param name="description">The description of this future <see cref="Command"/>.</param>
        /// <returns>Returns this <see cref="CommandBaseBuilder"/> as an <see cref="ICommandBaseDescription"/>, prompting the user for a <see cref="Telegram.Bot.Types.Enums.ParseMode"/>.</returns>
        public ICommandBaseDescription HelpDescription(ISendableMenu helpMessage)
        {
            InHelpDescription = helpMessage;
            return this;
        }
        /// <summary>
        /// Adds an <see cref="InlineKeyboardButton"/> to this <see cref="CommandBaseBuilder"/>. Ends fluent building for this object (this is the final option availalble).
        /// </summary>
        /// <param name="button">The <see cref="InlineKeyboardButton"/> for this future <see cref="Command"/>.</param>
        public void InlineKeyboardButtonReference(InlineKeyboardButton button)
        {
            InButton = button;
        }
        #endregion

        //* This is for new features, to help separate them from the original implementation. *//
        #region Extensibility Support
        //
        #region Properties
        internal Permissions Permissions { get; private set; }
        internal ISendableMenu? ErrorMsg { get; private set; }
        internal StepContainer? StepInfo { get; private set; }
        #endregion
        //
        #region Methods
        internal void Set_CommandType(CommandType type) => CommandType = type;
        internal void Set_Permissions(PermissionsAttribute? p) => Permissions = p?.Permissions ?? Permissions.None;
        internal void Set_Steps(IEnumerable<MethodInfo> methods) { StepInfo = new StepContainer(methods); } // Pre-filtered in the CommandService class.
        #endregion
        //
        #endregion
    }
}
