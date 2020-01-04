using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types.ReplyMarkups;

namespace FluentCommands.Interfaces.KeyboardBuilders
{
    public interface IReplyMarkupable<TBuilder> where TBuilder : class, IFluentInterface
    {
        /// <summary>
        /// Constructs an <see cref="IKeyboardBuilder{TBuilder}"/> for this <see cref="Command"/> or <see cref="Menus.MenuItem"/>.
        /// </summary>
        /// <returns>Returns this <see cref="TBuilder"/>, removing this option from the fluent builder.</returns>
        IKeyboardBuilder<TBuilder> ReplyMarkup();
        /// <summary>
        /// Adds an <see cref="InlineKeyboardMarkup"/> to the <see cref="Command"/> or <see cref="Menus.MenuItem"/>.
        /// </summary>
        /// <param name="markup">The <see cref="InlineKeyboardMarkup"/> for this future <see cref="Command"/> or <see cref="Menus.MenuItem"/>.</param>
        /// <returns>Returns this <see cref="TBuilder"/>, removing this option from the fluent builder.</returns>
        TBuilder ReplyMarkup(InlineKeyboardMarkup markup);
        /// <summary>
        /// Adds a <see cref="ReplyKeyboardMarkup"/> to the <see cref="Command"/> or <see cref="Menus.MenuItem"/>.
        /// </summary>
        /// <param name="markup">The <see cref="ReplyKeyboardMarkup"/> for this future <see cref="Command"/> or <see cref="Menus.MenuItem"/>.</param>
        /// <returns>Returns this <see cref="TBuilder"/>, removing this option from the fluent builder.</returns>
        TBuilder ReplyMarkup(ReplyKeyboardMarkup markup);
        /// <summary>
        /// Adds a <see cref="ForceReplyMarkup"/> to the <see cref="Command"/> or <see cref="Menus.MenuItem"/>.
        /// </summary>
        /// <param name="markup">The <see cref="ForceReplyMarkup"/> for this future <see cref="Command"/> or <see cref="Menus.MenuItem"/>.</param>
        /// <param name="selectve"></param>
        /// <returns>Returns this <see cref="TBuilder"/>, removing this option from the fluent builder.</returns>
        TBuilder ReplyMarkup(ForceReplyMarkup markup, bool selective = false);
        /// <summary>
        /// Adds a <see cref="ReplyKeyboardRemove"/> to the <see cref="Command"/> or <see cref="Menus.MenuItem"/>.
        /// </summary>
        /// <param name="markup">The <see cref="ReplyKeyboardRemove"/> for this future <see cref="Command"/> or <see cref="Menus.MenuItem"/>.</param>
        /// <param name="selectve"></param>
        /// <returns>Returns this <see cref="TBuilder"/>, removing this option from the fluent builder.</returns>
        TBuilder ReplyMarkup(ReplyKeyboardRemove markup, bool selective = false);
    }
}
