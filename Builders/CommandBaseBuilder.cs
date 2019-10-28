using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using FluentCommands.Exceptions;
using FluentCommands.Interfaces;
using FluentCommands.Interfaces.BaseBuilders;
using FluentCommands.Interfaces.KeyboardBuilders;
using FluentCommands.CommandTypes;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace FluentCommands.Builders
{
    /// <summary>
    /// Builder responsible for creating <see cref="CommandBase"/> objects, that will be used to create full <see cref="Command"/> objects for the <see cref="CommandService"/>.
    /// </summary>
    public sealed class CommandBaseBuilder : ICommandBaseBuilder,
        ICommandBaseAliases, ICommandDescriptionBuilder, ICommandBaseKeyboard, ICommandBaseDescription, IReplyMarkupable<ICommandBaseKeyboard>, IKeyboardBuilder<ICommandBaseKeyboard>,
        IFluentInterface
    {
        /// <summary>
        /// Gets the name of this <see cref="Command"/>.
        /// </summary>
        internal string Name { get; private set; }
        /// <summary>
        /// Gets the Aliases (other names) for this <see cref="Command"/>.
        /// </summary>
        internal string[] InAliases { get; private set; } = Array.Empty<string>();
        /// <summary>
        /// Gets the Description for this <see cref="Command"/>.
        /// </summary>
        internal string InDescription { get; private set; } = "There is no description for this command.";
        /// <summary>
        /// Gets the <see cref="Telegram.Bot.Types.Enums.ParseMode"/> for this <see cref="Command"/>. Used for the <see cref="Command.Description"/>).
        /// </summary>
        internal ParseMode InParseMode { get; private set; } = ParseMode.Default;
        /// <summary>
        /// Gets the <see cref="IKeyboardButton"/> for this <see cref="Command"/>. Used to call this command via Keyboard Markups, such as <see cref="InlineKeyboardMarkup"/> and <see cref="ReplyKeyboardMarkup"/>).
        /// </summary>
        internal IKeyboardButton InButton { get; private set; } = null;
        /// <summary>
        /// Gets the <see cref="KeyboardBuilder"/> for this <see cref="Command"/>
        /// </summary>
        internal KeyboardBuilder KeyboardInfo { get; private set; } = null;

        /// <summary>
        /// Instantiates a new <see cref="CommandBaseBuilder"/>, which will be used to construct a <see cref="Command"/> for this Module.
        /// </summary>
        /// <param name="name">The name of this future <see cref="Command"/>.</param>
        internal CommandBaseBuilder(string name) => Name = name;

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
        /// <returns>Returns this <see cref="CommandBaseBuilder"/> as an <see cref="ICommandDescriptionBuilder"/>, prompting the user for a <see cref="Telegram.Bot.Types.Enums.ParseMode"/>.</returns>
        public ICommandDescriptionBuilder HelpDescription(string description)
        {
            InDescription = description;
            return this;
        }
        /// <summary>
        /// Adds the <see cref="Telegram.Bot.Types.Enums.ParseMode"/> for the description of this <see cref="CommandBaseBuilder"/>.
        /// </summary>
        /// <param name="parseMode">The <see cref="Telegram.Bot.Types.Enums.ParseMode"/> for the description of this future <see cref="Command"/>.</param>
        /// <returns>Returns this <see cref="CommandBaseBuilder"/> as an <see cref="ICommandBaseDescription"/>, removing this option from the fluent builder.</returns>
        public ICommandBaseDescription UsingParseMode(ParseMode parseMode)
        {
            InParseMode = parseMode;
            return this;
        }
        /// <summary>
        /// Constructs a <see cref="KeyboardBuilder"/> for this <see cref="CommandBaseBuilder"/>.
        /// </summary>
        /// <returns>Returns this <see cref="CommandBaseBuilder"/> as an <see cref="IKeyboardBuilder"/>, removing this option from the fluent builder.</returns>
        public IKeyboardBuilder<ICommandBaseKeyboard> ReplyMarkup() => this;
        /// <summary>
        /// Adds a <see cref="ReplyKeyboardRemove"/> to this <see cref="Command"/>.
        /// <para>Selective: Use this parameter if you want to show the keyboard to specific users only.</para>
        /// </summary>
        /// <param name="markup"></param>
        /// <param name="selective"></param>
        /// <returns></returns>
        public ICommandBaseKeyboard ReplyMarkup(ForceReplyMarkup markup, bool selective = false)
        {
            KeyboardInfo = new KeyboardBuilder(markup, selective);
            return this;
        }
        /// <summary>
        /// Adds a <see cref="ForceReplyMarkup"/> to this <see cref="Command"/>.
        /// <para>Selective: Use this parameter if you want to show the keyboard to specific users only.</para>
        /// </summary>
        /// <param name="markup"></param>
        /// <param name="selective"></param>
        /// <returns></returns>
        public ICommandBaseKeyboard ReplyMarkup(ReplyKeyboardRemove markup, bool selective = false)
        {
            KeyboardInfo = new KeyboardBuilder(markup, selective);
            return this;
        }
        /// <summary>
        /// Adds an <see cref="InlineKeyboardMarkup"/> to the <see cref="KeyboardBuilder"/> of this <see cref="CommandBaseBuilder"/>.
        /// </summary>
        /// <param name="markup">The <see cref="InlineKeyboardMarkup"/> for this future <see cref="Command"/>.</param>
        /// <returns>Returns this <see cref="CommandBaseBuilder"/> as an <see cref="ICommandBaseKeyboard"/>, removing this option from the fluent builder.</returns>
        public ICommandBaseKeyboard ReplyMarkup(InlineKeyboardMarkup markup)
        {
            var storage = new KeyboardBuilder();
            foreach (var row in markup.InlineKeyboard)
            {
                storage.AddRow(row.ToArray());
            }
            KeyboardInfo = storage;
            return this;
        }
        /// <summary>
        /// Adds a <see cref="ReplyKeyboardMarkup"/> to the <see cref="KeyboardBuilder"/> of this <see cref="CommandBaseBuilder"/>.
        /// </summary>
        /// <param name="markup">The <see cref="ReplyKeyboardMarkup"/> for this future <see cref="Command"/>.</param>
        /// <returns>Returns this <see cref="CommandBaseBuilder"/> as an <see cref="ICommandBaseKeyboard"/>, removing this option from the fluent builder.</returns>
        public ICommandBaseKeyboard ReplyMarkup(ReplyKeyboardMarkup markup)
        {
            var storage = new KeyboardBuilder();
            foreach (var row in markup.Keyboard)
            {
                storage.AddRow(row.ToArray());
            }
            storage.BuildWithSettings(markup.OneTimeKeyboard, markup.ResizeKeyboard, markup.Selective);
            KeyboardInfo = storage;
            return this;
        }
        /// <summary>
        /// Builds a new <see cref="InlineKeyboardMarkup"/> for this <see cref="CommandBaseBuilder"/>.
        /// </summary>
        /// <param name="buildAction">The build action delegate for this <see cref="KeyboardBuilder"/>>.</param>
        /// <returns>Returns this <see cref="CommandBaseBuilder"/> as an <see cref="ICommandBaseKeyboard"/>, removing this option from the fluent builder.</returns>
        public ICommandBaseKeyboard Inline(Action<IInlineKeyboardBuilder> buildAction)
        {
            KeyboardInfo = new KeyboardBuilder();
            buildAction(KeyboardInfo);
            return this;
        }
        /// <summary>
        /// Builds a new <see cref="ReplyKeyboardMarkup"/> for this <see cref="CommandBaseBuilder"/>.
        /// </summary>
        /// <param name="buildAction">The build action delegate for this <see cref="KeyboardBuilder"/>.</param>
        /// <returns>Returns this <see cref="CommandBaseBuilder"/> as an <see cref="ICommandBaseKeyboard"/>, removing this option from the fluent builder.</returns>
        public ICommandBaseKeyboard Reply(Action<IReplyKeyboardBuilder> buildAction)
        {
            KeyboardInfo = new KeyboardBuilder();
            buildAction(KeyboardInfo);
            return this;
        }
        /// <summary>
        /// Adds a <see cref="ReplyKeyboardRemove"/> to this <see cref="Command"/>.
        /// <para>Selective: Use this parameter if you want to show the keyboard to specific users only.</para>
        /// </summary>
        /// <param name="selective"></param>
        /// <returns></returns>
        public ICommandBaseKeyboard Remove(bool selective = false)
        {
            KeyboardInfo = new KeyboardBuilder(new ReplyKeyboardRemove(), selective);
            return this;
        }
        /// <summary>
        /// Adds a <see cref="ForceReplyMarkup"/> to this <see cref="Command"/>.
        /// <para>Selective: Use this parameter if you want to show the keyboard to specific users only.</para>
        /// </summary>
        /// <param name="selective"></param>
        /// <returns></returns>
        public ICommandBaseKeyboard ForceReply(bool selective = false)
        {
            KeyboardInfo = new KeyboardBuilder(new ForceReplyMarkup(), selective);
            return this;
        }
        /// <summary>
        /// Adds an <see cref="IKeyboardButton"/> to this <see cref="CommandBaseBuilder"/>. Ends fluent building for this object (this is the final option availalble).
        /// </summary>
        /// <param name="button">The <see cref="IKeyboardButton"/> for this future <see cref="Command"/>.</param>
        public void KeyboardButtonReference(IKeyboardButton button)
        {
            InButton = button;
        }
        /// <summary>
        /// Converts a <see cref="CommandBaseBuilder"/> into a <see cref="CommandBase"/>, to be formed into a <see cref="Command"/>.
        /// </summary>
        /// <returns>Returns the converted <see cref="CommandBase"/>.</returns>
        internal CommandBase ConvertToBase()
        {
            return new CommandBase
            {
                Aliases = this.InAliases,
                Button = this.InButton,
                Description = this.InDescription,
                KeyboardInfo = this.KeyboardInfo,
                Name = this.Name,
                ParseMode = this.InParseMode
            };
        }


    }
}
