using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types.ReplyMarkups;

namespace FluentCommands.Interfaces.KeyboardBuilders
{
    public interface IReplyKeyboardBuilder : IFluentInterface
    {
        /// <summary>
        /// Indexer that creates a reference to a <see cref="Command"/> object's <see cref="IKeyboardButton"/>.
        /// </summary>
        /// <param name="commandName">The name of this <see cref="Command"/> to reference for its <see cref="IKeyboardButton"/>.</param>
        /// <returns>Returns a reference to this <see cref="Command"/> object's <see cref="IKeyboardButton"/>.</returns>
        KeyboardButtonReference this[string commandName] { get; }

        /// <summary>
        /// Adds a row of buttons to the keyboard builder.
        /// </summary>
        /// <param name="buttons">The buttons to add to this row.</param>
        /// <returns>Returns this <see cref="IKeyboardBuilder"/>, so that the user can add more rows.</returns>
        IReplyKeyboardBuilder AddRow(params KeyboardButton[] buttons);

        /// <summary>
        /// Finalizes the keyboard with optional settings for the <see cref="ReplyKeyboardMarkup"/> that will be generated from this builder.
        /// </summary>
        /// <param name="oneTimeKeyboard">The boolean that will be assigned to <see cref="ReplyKeyboardMarkup.OneTimeKeyboard"/>.</param>
        /// <param name="resizeKeyboard">The boolean that will be assigned to <see cref="ReplyKeyboardMarkup.ResizeKeyboard"/>.</param>
        /// <param name="selective">The boolean that will be assigned to <see cref="ReplyMarkupBase.Selective"/>.</param>
        void BuildWithSettings(bool oneTimeKeyboard = false, bool resizeKeyboard = false, bool selective = false);
    }
}
