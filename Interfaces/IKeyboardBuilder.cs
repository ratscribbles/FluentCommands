using System;
using System.Collections.Generic;
using System.Text;
using FluentCommands.Builders;
using Telegram.Bot.Types.ReplyMarkups;

namespace FluentCommands.Interfaces
{
    /// <summary>
    /// Fluent interface for creating Keyboard Builders (for <see cref="InlineKeyboardBuilder"/> and <see cref="ReplyKeyboardBuilder"/>).
    /// </summary>
    /// <typeparam name="TBuilder">The keyboard builder (either <see cref="InlineKeyboardBuilder"/> or <see cref="ReplyKeyboardBuilder"/>).</typeparam>
    /// <typeparam name="TButton">The <see cref="IKeyboardButton"/> (either <see cref="InlineKeyboardButton"/> or <see cref="KeyboardButton"/>).</typeparam>
    public interface IKeyboardBuilder<TBuilder, TButton> where TBuilder : IKeyboardBuilder<TBuilder, TButton> where TButton : IKeyboardButton
    {
        /// <summary>
        /// Adds a row of buttons to the keyboard builder.
        /// </summary>
        /// <param name="buttons">The buttons to add to this row.</param>
        /// <returns>Returns this <see cref="IKeyboardBuilder{TBuilder, TButton}"/>, so that the user can add more rows.</returns>
        TBuilder AddRow(params TButton[] buttons);
    }
}
