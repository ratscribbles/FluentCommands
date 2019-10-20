using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using FluentCommands.Interfaces;
using FluentCommands.Exceptions;

namespace FluentCommands.Builders
{
    /// <summary>
    /// Builder stored within <see cref="KeyboardBuilder"/>, containing information to create an <see cref="InlineKeyboardMarkup"/>.
    /// </summary>
    public sealed class InlineKeyboardBuilder : IKeyboardBuilder<InlineKeyboardBuilder, InlineKeyboardButton>, IFluentInterface
    {
        /// <summary>
        /// Gets the <see cref="InlineKeyboardButton"/> rows to be used to create an <see cref="InlineKeyboardMarkup"/>.
        /// </summary>
        internal List<InlineKeyboardButton[]> Rows { get; private set; } = new List<InlineKeyboardButton[]>();

        /// <summary>
        /// Instantiates a new <see cref="InlineKeyboardBuilder"/>. Typically serves as <see cref="KeyboardBuilder.Inline"/>.
        /// </summary>
        internal InlineKeyboardBuilder() { }

        /// <summary>
        /// Adds a row of <see cref="InlineKeyboardButton"/>[] to <see cref="Rows"/>.
        /// <para>Used to create an <see cref="InlineKeyboardMarkup"/> for this <see cref="Command"/>.</para>
        /// </summary>
        /// <param name="buttons">The buttons to be added to <see cref="Rows"/>.</param>
        /// <exception cref="InvalidKeyboardRowException">Throws if conditions for <see cref="InlineKeyboardMarkup"/> are not met: no buttons; more than eight buttons; more than 13 rows; more than 4 buttons on the 13th row.</exception>
        /// <returns>Returns this <see cref="InlineKeyboardBuilder"/>, allowing you to continue adding rows.</returns>
        public InlineKeyboardBuilder AddRow(params InlineKeyboardButton[] buttons)
        {
            Rows.Add(buttons);

            // Exceptions...
            if (Rows.Count == 13 && buttons.Length > 4) throw new InvalidKeyboardRowException("An Inline Keyboard's 13th row may only contain 4 buttons.");
            else if (Rows.Count > 13) throw new InvalidKeyboardRowException("Inline Keyboards may only have a maximum of 13 rows.");
            else if (buttons.Length <= 0) throw new InvalidKeyboardRowException("Inline Keyboard rows must contain at least one button.");
            else if (buttons.Length > 8) throw new InvalidKeyboardRowException("Inline Keyboard rows may only have a maximum of 8 buttons.");

            return this;
        }
    }
}
