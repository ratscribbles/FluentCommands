using System;
using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using FluentCommands.Exceptions;
using FluentCommands.Interfaces;
using FluentCommands.Interfaces.BaseBuilders;
using FluentCommands.Interfaces.KeyboardBuilders;
using Telegram.Bot.Types.ReplyMarkups;
using FluentCommands.KeyboardTypes;

namespace FluentCommands.Builders
{
    /// <summary>
    /// Parent builder of <see cref="InlineKeyboardBuilder"/> and <see cref="ReplyKeyboardBuilder"/>. Stores keyboard information provided to a <see cref="Command"/> or <see cref="Menus.Menu"/> object.
    /// </summary>
    public class KeyboardBuilder : IInlineKeyboardBuilder, IReplyKeyboardBuilder, IFluentInterface
    {
        private bool _rowsUpdated = false;

        /// <summary>
        /// Gets the <see cref="InlineKeyboardButton"/> rows to be used to create an <see cref="InlineKeyboardMarkup"/>.
        /// </summary>
        internal List<InlineKeyboardButton[]> InlineRows { get; private set; } = new List<InlineKeyboardButton[]>();
        /// <summary>
        /// Gets the <see cref="KeyboardButton"/> rows to be used to create a <see cref="ReplyKeyboardMarkup"/>.
        /// </summary>
        internal List<KeyboardButton[]> ReplyRows { get; private set; } = new List<KeyboardButton[]>();
        /// <summary>
        /// Gets the <see cref="ReplyKeyboardRemove"/> to be used for this <see cref="Command"/> or <see cref="Menus.Menu"/>.
        /// </summary>
        internal ReplyKeyboardRemove? ReplyRemove { get; private set; } = null;
        /// <summary>
        /// Gets the <see cref="ForceReplyMarkup"/> to be used for this <see cref="Command"/> or <see cref="Menus.Menu"/>.
        /// </summary>
        internal ForceReplyMarkup? ForceReply { get; private set; } = null;
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
        /// Instantiates a new <see cref="KeyboardBuilder"/>. Contains information to construct an <see cref="IKeyboardBuilder{TBuilder}"/> of the correct type for this <see cref="Command"/> or <see cref="Menus.Menu"/>.
        /// </summary>
        internal KeyboardBuilder() { }

        /// <summary>
        /// Instantiates a new <see cref="KeyboardBuilder"/>. Contains information to construct an <see cref="IKeyboardBuilder{TBuilder}"/> of the correct type for this <see cref="Command"/> or <see cref="Menus.Menu"/>.
        /// </summary>
        /// <param name="forceReplyMarkup"></param>
        /// <param name="selective"></param>
        internal KeyboardBuilder(ForceReplyMarkup forceReplyMarkup, bool selective = false)
        {
            ForceReply = forceReplyMarkup;
            Selective = selective;
        }

        /// <summary>
        /// Instantiates a new <see cref="KeyboardBuilder"/>. Contains information to construct an <see cref="IKeyboardBuilder{TBuilder}"/> of the correct type for this <see cref="Command"/> or <see cref="Menus.Menu"/>.
        /// </summary>
        /// <param name="replyKeyboardRemove"></param>
        /// <param name="selective"></param>
        internal KeyboardBuilder(ReplyKeyboardRemove replyKeyboardRemove, bool selective = false)
        {
            ReplyRemove = replyKeyboardRemove;
            Selective = selective;
        }

        /// <summary>
        /// Adds a row of <see cref="InlineKeyboardButton"/>[] to <see cref="InlineRows"/>.
        /// <para>Used to create an <see cref="InlineKeyboardMarkup"/> for this <see cref="Command"/>.</para>
        /// </summary>
        /// <param name="buttons">The buttons to be added to <see cref="InlineRows"/>.</param>
        /// <exception cref="InvalidKeyboardRowException">Throws if conditions for <see cref="InlineKeyboardMarkup"/> are not met: no buttons; more than eight buttons; more than 13 rows; more than 4 buttons on the 13th row.</exception>
        /// <returns>Returns this <see cref="InlineKeyboardBuilder"/>, allowing you to continue adding rows.</returns>
        public IInlineKeyboardBuilder AddRow(params InlineKeyboardButton[] buttons)
        {
            InlineRows.Add(buttons);

            // Exceptions...
            if (InlineRows.Count == 13 && buttons.Length > 4) throw new InvalidKeyboardRowException("An Inline Keyboard's 13th row may only contain 4 buttons.");
            else if (InlineRows.Count > 13) throw new InvalidKeyboardRowException("Inline Keyboards may only have a maximum of 13 rows.");
            else if (buttons.Length <= 0) throw new InvalidKeyboardRowException("Inline Keyboard rows must contain at least one button.");
            else if (buttons.Length > 8) throw new InvalidKeyboardRowException("Inline Keyboard rows may only have a maximum of 8 buttons.");

            return this;
        }

        /// <summary>
        /// Adds a row of <see cref="KeyboardButton"/>[] to <see cref="ReplyRows"/>.
        /// <para>Used to create an <see cref="ReplyKeyboardMarkup"/> for this <see cref="Command"/>.</para>
        /// </summary>
        /// <param name="buttons">The buttons to be added to <see cref="ReplyRows"/>.</param>
        /// <exception cref="InvalidKeyboardRowException">Throws if conditions for <see cref="ReplyKeyboardMarkup"/> are not met: no buttons; more than 12 buttons; more than 25 rows.</exception>
        /// <returns>Returns this <see cref="ReplyKeyboardBuilder"/>, allowing you to continue adding rows.</returns>
        public IReplyKeyboardBuilder AddRow(params KeyboardButton[] buttons)
        {
            ReplyRows.Add(buttons);

            // Exceptions...
            if (ReplyRows.Count > 25) throw new InvalidKeyboardRowException("Reply Keyboards may only have a maximum of 25 rows.");
            else if (buttons.Length <= 0) throw new InvalidKeyboardRowException("Reply Keyboard rows must contain at least one button.");
            else if (buttons.Length > 12) throw new InvalidKeyboardRowException("Reply Keyboard rows may only have a maximum of 12 buttons.");

            return this;
        }

        /// <summary>
        /// Finalizes the keyboard with optional settings for the <see cref="ReplyKeyboardMarkup"/> that will be generated from this builder.
        /// <para>OneTimeKeyboard: Requests clients to hide the keyboard as soon as it's been used. </para>
        /// <para>ResizeKeyboard: Requests clients to resize the keyboard vertically for optimal fit.</para>
        /// <para>Selective: Use this parameter if you want to show the keyboard to specific users only.</para>
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

        /// <summary>
        /// Sets inline rows. Explicit method meant to be called ONLY ONCE when updating keyboard rows.
        /// </summary>
        internal void UpdateInline(List<InlineKeyboardButton[]> rows)
        {
            if (!_rowsUpdated) { InlineRows = rows; _rowsUpdated = true; }
        }

        /// <summary>
        /// Sets reply rows. Explicit method meant to be called ONLY ONCE when updating keyboard rows.
        /// </summary>
        internal void UpdateReply(List<KeyboardButton[]> rows)
        {
            if (!_rowsUpdated) { ReplyRows = rows; _rowsUpdated = true; }
        }

        public static implicit operator InlineKeyboardMarkup(KeyboardBuilder k)
        {
            var rows = CommandService.UpdateKeyboardRows(k?.InlineRows ?? new List<InlineKeyboardButton[]>());
            return new InlineKeyboardMarkup(rows);
        }

        public static implicit operator ReplyKeyboardMarkup(KeyboardBuilder k)
        {
            var rows = CommandService.UpdateKeyboardRows(k?.ReplyRows ?? new List<KeyboardButton[]>());
            return new ReplyKeyboardMarkup(rows);
        }
    }
}
