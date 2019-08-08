using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using FluentCommands.Interfaces;
using FluentCommands.Interfaces.BaseBuilders;
using FluentCommands.CommandTypes;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace FluentCommands.Builders
{
    /// <summary>
    /// Builder responsible for creating <see cref="CommandBase"/> objects, that will be used to create full <see cref="Command"/> objects for the <see cref="CommandService"/>.
    /// </summary>
    /// <typeparam name="TModule">The class that represents a Module for the <see cref="CommandService"/> to construct <see cref="Command"/> objects from.</typeparam>
    public sealed partial class CommandBaseBuilder<TModule> : ICommandBaseBuilder<TModule>,
        ICommandBaseAliases<TModule>, ICommandDescriptionBuilder<TModule>, ICommandBaseKeyboard<TModule>, ICommandBaseDescription<TModule>,
        IFluentInterface where TModule : class
    {
        /// <summary>
        /// Gets the <see cref="Type"/> of this Module for the <see cref="CommandService"/>.
        /// </summary>
        internal Type Module { get; private set; }
        /// <summary>
        /// Gets the name of this <see cref="Command"/>.
        /// </summary>
        internal string Name { get; private set; }
        /// <summary>
        /// Gets the Aliases (other names) for this <see cref="Command"/>.
        /// </summary>
        internal string[] Aliases { get; private set; } = Array.Empty<string>();
        /// <summary>
        /// Gets the Description for this <see cref="Command"/>.
        /// </summary>
        internal string Description { get; private set; } = "There is no description for this command.";
        /// <summary>
        /// Gets the <see cref="Telegram.Bot.Types.Enums.ParseMode"/> for this <see cref="Command"/>. Used for the <see cref="Command.Description"/>).
        /// </summary>
        internal ParseMode ParseMode { get; private set; } = ParseMode.Default;
        /// <summary>
        /// Gets the <see cref="IKeyboardButton"/> for this <see cref="Command"/>. Used to call this command via Keyboard Markups, such as <see cref="InlineKeyboardMarkup"/> and <see cref="ReplyKeyboardMarkup"/>).
        /// </summary>
        internal IKeyboardButton Button { get; private set; } = null;
        /// <summary>
        /// Gets the <see cref="KeyboardBuilder"/> for this <see cref="Command"/>
        /// </summary>
        internal KeyboardBuilder KeyboardInfo { get; private set; } = null;

        /// <summary>
        /// Instantiates a new <see cref="CommandBaseBuilder{TModule}"/>, which will be used to construct a <see cref="Command"/> for this Module.
        /// </summary>
        /// <param name="name">The name of this future <see cref="Command"/>.</param>
        internal CommandBaseBuilder(string name) { Name = name; Module = typeof(TModule); }

        /// <summary>
        /// Adds aliases (alternate names) to this <see cref="CommandBaseBuilder{TModule}"/>.
        /// </summary>
        /// <param name="aliases">The alternate names for this future <see cref="Command"/>.</param>
        /// <returns>Returns this <see cref="CommandBaseBuilder{TModule}"/> as an <see cref="ICommandBaseAliases{TModule}"/>, removing this option from the fluent builder.</returns>
        public ICommandBaseAliases<TModule> HasAliases(params string[] aliases)
        {
            Aliases = aliases;
            return this;
        }
        /// <summary>
        /// Adds a description to this <see cref="CommandBaseBuilder{TModule}"/>.
        /// </summary>
        /// <param name="description">The description of this future <see cref="Command"/>.</param>
        /// <returns>Returns this <see cref="CommandBaseBuilder{TModule}"/> as an <see cref="ICommandDescriptionBuilder{TModule}"/>, prompting the user for a <see cref="Telegram.Bot.Types.Enums.ParseMode"/>.</returns>
        public ICommandDescriptionBuilder<TModule> HasHelpDescription(string description)
        {
            Description = description;
            return this;
        }
        /// <summary>
        /// Adds the <see cref="Telegram.Bot.Types.Enums.ParseMode"/> for the description of this <see cref="CommandBaseBuilder{TModule}"/>.
        /// </summary>
        /// <param name="parseMode">The <see cref="Telegram.Bot.Types.Enums.ParseMode"/> for the description of this future <see cref="Command"/>.</param>
        /// <returns>Returns this <see cref="CommandBaseBuilder{TModule}"/> as an <see cref="ICommandBaseDescription{TModule}"/>, removing this option from the fluent builder.</returns>
        public ICommandBaseDescription<TModule> WithParseMode(ParseMode parseMode)
        {
            ParseMode = parseMode;
            return this;
        }
        /// <summary>
        /// Constructs a <see cref="KeyboardBuilder"/> for this <see cref="CommandBaseBuilder{TModule}"/>.
        /// </summary>
        /// <param name="buildAction">Delegate that constructs a <see cref="KeyboardBuilder"/> for this future <see cref="Command"/>.</param>
        /// <returns>Returns this <see cref="CommandBaseBuilder{TModule}"/> as an <see cref="ICommandBaseKeyboard{TModule}"/>, removing this option from the fluent builder.</returns>
        public ICommandBaseKeyboard<TModule> HasKeyboard(Action<KeyboardBuilder> buildAction)
        {
            var keyboard = new KeyboardBuilder(typeof(TModule));
            buildAction(keyboard);
            KeyboardInfo = keyboard;
            return this;
        }
        /// <summary>
        /// Adds an <see cref="InlineKeyboardMarkup"/> to the <see cref="KeyboardBuilder"/> of this <see cref="CommandBaseBuilder{TModule}"/>.
        /// </summary>
        /// <param name="markup">The <see cref="InlineKeyboardMarkup"/> for this future <see cref="Command"/>.</param>
        /// <returns>Returns this <see cref="CommandBaseBuilder{TModule}"/> as an <see cref="ICommandBaseKeyboard{TModule}"/>, removing this option from the fluent builder.</returns>
        public ICommandBaseKeyboard<TModule> HasKeyboard(InlineKeyboardMarkup markup)
        {
            var storage = new KeyboardBuilder(typeof(TModule));
            foreach (var row in markup.InlineKeyboard)
            {
                storage.AddRow(row.ToArray());
            }
            KeyboardInfo = storage;
            return this;
        }
        /// <summary>
        /// Adds a <see cref="ReplyKeyboardMarkup"/> to the <see cref="KeyboardBuilder"/> of this <see cref="CommandBaseBuilder{TModule}"/>.
        /// </summary>
        /// <param name="markup">The <see cref="ReplyKeyboardMarkup"/> for this future <see cref="Command"/>.</param>
        /// <returns>Returns this <see cref="CommandBaseBuilder{TModule}"/> as an <see cref="ICommandBaseKeyboard{TModule}"/>, removing this option from the fluent builder.</returns>
        public ICommandBaseKeyboard<TModule> HasKeyboard(ReplyKeyboardMarkup markup)
        {
            var storage = new KeyboardBuilder(typeof(TModule));
            foreach (var row in markup.Keyboard)
            {
                storage.AddRow(row.ToArray());
            }
            storage.Reply.BuildWithSettings(markup.OneTimeKeyboard, markup.ResizeKeyboard, markup.Selective);
            KeyboardInfo = storage;
            return this;
        }
        /// <summary>
        /// Adds an <see cref="IKeyboardButton"/> to this <see cref="CommandBaseBuilder{TModule}"/>. Ends fluent building for this object (this is the final option availalble).
        /// </summary>
        /// <param name="button">The <see cref="IKeyboardButton"/> for this future <see cref="Command"/>.</param>
        public void HasKeyboardButton(IKeyboardButton button)
        {
            Button = button;
        }

        //! Operators !//



        /// <summary>
        /// Explicitly converts a <see cref="CommandBaseBuilder{TModule}"/> into a <see cref="CommandBase"/>, to be formed into a <see cref="Command"/>.
        /// </summary>
        /// <param name="b">The <see cref="CommandBaseBuilder{TModule}"/> to be converted into a <see cref="CommandBase"/>.</param>
        public static explicit operator CommandBase(CommandBaseBuilder<TModule> b)
        {
            return new CommandBase
            {
                Aliases = b.Aliases,
                Button = b.Button,
                Description = b.Description,
                KeyboardInfo = b.KeyboardInfo,
                Module = b.Module,
                Name = b.Name,
                ParseMode = b.ParseMode
            };
        }
    }
}
