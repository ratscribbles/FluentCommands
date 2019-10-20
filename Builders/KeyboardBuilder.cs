using System;
using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using FluentCommands.Interfaces;
using Telegram.Bot.Types.ReplyMarkups;

namespace FluentCommands.Builders
{
    /// <summary>
    /// Parent builder of <see cref="InlineKeyboardBuilder"/> and <see cref="ReplyKeyboardBuilder"/>. Stores keyboard information provided to a <see cref="Command"/> object.
    /// </summary>
    public class KeyboardBuilder : IFluentInterface
    {
        /// <summary>
        /// The <see cref="InlineKeyboardBuilder"/> containing information to create an <see cref="InlineKeyboardMarkup"/> for a <see cref="Command"/> object.
        /// </summary>
        internal InlineKeyboardBuilder Inline { get; set; } = null;
        /// <summary>
        /// The <see cref="ReplyKeyboardBuilder"/> containing information to create an <see cref="ReplyKeyboardMarkup"/> for a <see cref="Command"/> object.
        /// </summary>
        internal ReplyKeyboardBuilder Reply { get; set; } = null;

        /// <summary>
        /// Indexer that creates a reference to a <see cref="Command"/> object's <see cref="IKeyboardButton"/>.
        /// </summary>
        /// <param name="commandName">The name of this <see cref="Command"/> to reference for its <see cref="IKeyboardButton"/>.</param>
        /// <returns>Returns a reference to this <see cref="Command"/> object's <see cref="IKeyboardButton"/>.</returns>
        public KeyboardButtonReference this[string commandName]
        {
            get
            {
                return new KeyboardButtonReference(commandName);
            }
        }

        /// <summary>
        /// Instantiates a new <see cref="KeyboardBuilder"/>. Contains information to construct an <see cref="IKeyboardBuilder{TBuilder, TButton}"/> for this <see cref="Command"/>.
        /// </summary>
        internal KeyboardBuilder() { }

        /// <summary>
        /// Adds a row to the contained <see cref="InlineKeyboardBuilder"/>, and fluently paths to the <see cref="InlineKeyboardBuilder"/>.
        /// </summary>
        /// <param name="buttons"><see cref="InlineKeyboardButton"/> objects to be added to this <see cref="InlineKeyboardBuilder"/>.</param>
        /// <returns>Returns this <see cref="InlineKeyboardBuilder"/>.</returns>
        public InlineKeyboardBuilder AddRow(params InlineKeyboardButton[] buttons)
        {
            Inline = new InlineKeyboardBuilder();
            Inline.AddRow(buttons);
            return Inline;
        }

        /// <summary>
        /// Adds a row to the contained <see cref="ReplyKeyboardBuilder"/>, and fluently paths to the <see cref="ReplyKeyboardBuilder"/>.
        /// </summary>
        /// <param name="buttons"><see cref="ReplyKeyboardButton"/> objects to be added to this <see cref="ReplyKeyboardBuilder"/>.</param>
        /// <returns>Returns this <see cref="ReplyKeyboardBuilder"/>.</returns>
        public ReplyKeyboardBuilder AddRow(params KeyboardButton[] buttons)
        {
            Reply = new ReplyKeyboardBuilder();
            Reply.AddRow(buttons);
            return Reply;
        }
    }
}
