using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types.ReplyMarkups;

namespace FluentCommands.Interfaces.KeyboardBuilders
{
    public interface IReplyMarkupableForceInline<TBuilder> where TBuilder : class, IFluentInterface
    {
        /// <summary>
        /// Constructs an <see cref="IKeyboardBuilder{TBuilder}"/> for this <see cref="Command"/> or <see cref="Menus.Menu"/>.
        /// </summary>
        /// <returns>Returns this <see cref="TBuilder"/>, removing this option from the fluent builder.</returns>
        IKeyboardBuilderForceInline<TBuilder> ReplyMarkup();
        /// <summary>
        /// Adds an <see cref="InlineKeyboardMarkup"/> to the <see cref="Command"/> or <see cref="Menus.Menu"/>.
        /// </summary>
        /// <param name="markup">The <see cref="InlineKeyboardMarkup"/> for this future <see cref="Command"/> or <see cref="Menus.Menu"/>.</param>
        /// <returns>Returns this <see cref="TBuilder"/>, removing this option from the fluent builder.</returns>
        TBuilder ReplyMarkup(InlineKeyboardMarkup markup);
    }
}
