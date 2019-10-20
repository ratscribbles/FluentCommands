using System;
using System.Collections.Generic;
using System.Text;
using FluentCommands.Interfaces;
using FluentCommands.Exceptions;
using Telegram.Bot.Types.ReplyMarkups;

namespace FluentCommands.Builders
{
    /// <summary>
    /// Builder stored within <see cref="KeyboardBuilder"/>, containing information to create a <see cref="ReplyKeyboardMarkup"/>.
    /// </summary>
    public sealed class ReplyKeyboardBuilder : IKeyboardBuilder<ReplyKeyboardBuilder, KeyboardButton>, IFluentInterface
    {
        /// <summary>
        /// Gets the <see cref="KeyboardButton"/> rows to be used to create a <see cref="ReplyKeyboardMarkup"/>.
        /// </summary>
        internal List<KeyboardButton[]> Rows { get; private set; } = new List<KeyboardButton[]>();
        /// <summary>
        /// Gets the boolean that will be assigned to <see cref="ReplyKeyboardMarkup.OneTimeKeyboard"/>.
        /// </summary>
        internal bool OneTimeKeyboard { get; private set; } = false;
        /// <summary>
        /// Gets the boolean that will be assigned to <see cref="ReplyKeyboardMarkup.ResizeKeyboard"/>.
        /// </summary>
        internal bool ResizeKeyboard { get; private set; } = false;
        /// <summary>
        /// Gets the boolean that will be assigned to <see cref="ReplyMarkupBase.Selective"/>.
        /// </summary>
        internal bool Selective { get; private set; } = false;

        /// <summary>
        /// Instantiates a new <see cref="ReplyKeyboardBuilder"/>. Typically serves as <see cref="KeyboardBuilder.Reply"/>.
        /// </summary>
        internal ReplyKeyboardBuilder() { }

        /// <summary>
        /// Adds a row of <see cref="KeyboardButton"/>[] to <see cref="Rows"/>.
        /// <para>Used to create an <see cref="ReplyKeyboardMarkup"/> for this <see cref="Command"/>.</para>
        /// </summary>
        /// <param name="buttons">The buttons to be added to <see cref="Rows"/>.</param>
        /// <exception cref="InvalidKeyboardRowException">Throws if conditions for <see cref="ReplyKeyboardMarkup"/> are not met: no buttons; more than 12 buttons; more than 25 rows.</exception>
        /// <returns>Returns this <see cref="ReplyKeyboardBuilder"/>, allowing you to continue adding rows.</returns>
        public ReplyKeyboardBuilder AddRow(params KeyboardButton[] buttons)
        {
            Rows.Add(buttons);

            // Exceptions...
            if (Rows.Count > 25) throw new InvalidKeyboardRowException("Reply Keyboards may only have a maximum of 25 rows.");
            else if (buttons.Length <= 0) throw new InvalidKeyboardRowException("Reply Keyboard rows must contain at least one button.");
            else if (buttons.Length > 12) throw new InvalidKeyboardRowException("Reply Keyboard rows may only have a maximum of 12 buttons.");

            return this;
        }

        /// <summary>
        /// Finalizes the keyboard with optional settings for the <see cref="ReplyKeyboardMarkup"/> that will be generated from this builder.
        /// </summary>
        /// <param name="oneTimeKeyboard">The boolean that will be assigned to <see cref="ReplyKeyboardMarkup.OneTimeKeyboard"/>.</param>
        /// <param name="resizeKeyboard">The boolean that will be assigned to <see cref="ReplyKeyboardMarkup.ResizeKeyboard"/>.</param>
        /// <param name="selective">The boolean that will be assigned to <see cref="ReplyMarkupBase.Selective"/>.</param>
        public void BuildWithSettings(bool oneTimeKeyboard = false, bool resizeKeyboard = false, bool selective = false)
        {
            OneTimeKeyboard = oneTimeKeyboard;
            ResizeKeyboard = resizeKeyboard;
            Selective = selective;
        }
    }
}
