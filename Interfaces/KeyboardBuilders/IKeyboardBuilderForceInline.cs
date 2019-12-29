using System;
using System.Collections.Generic;
using System.Text;
using FluentCommands.Interfaces.BaseBuilders;
using Telegram.Bot.Types.ReplyMarkups;

namespace FluentCommands.Interfaces.KeyboardBuilders
{
    /// <summary>
    /// Fluent interface for creating an  <see cref="IInlineKeyboardBuilder"/> only, due to this <see cref="Menus.MenuItem"/> or <see cref="Command"/> only accepting an <see cref="InlineKeyboardMarkup"/>.
    /// </summary>
    /// <typeparam name="TBuilder">The builder interface to return after building a keyboard.</typeparam>
    public interface IKeyboardBuilderForceInline<TBuilder> : IFluentInterface where TBuilder : class
    {
        /// <summary>
        /// Returns this <see cref="IKeyboardBuilder{TBuilder}"/> as one marked for <see cref="InlineKeyboardMarkup"/> objects.
        /// </summary>
        TBuilder Inline(Action<IInlineKeyboardBuilder> buildAction);
    }
}
