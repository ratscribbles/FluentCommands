using System;
using System.Collections.Generic;
using System.Text;
using FluentCommands.Builders;
using FluentCommands.Interfaces.BaseBuilders;
using Telegram.Bot.Types.ReplyMarkups;

namespace FluentCommands.Interfaces.KeyboardBuilders
{
    /// <summary>
    /// Fluent interface for creating Keyboard Builders (for <see cref="IInlineKeyboardBuilder"/> and <see cref="IReplyKeyboardBuilder"/>).
    /// </summary>
    /// <typeparam name="TBuilder">The builder interface to return after building a keyboard.</typeparam>
    public interface IKeyboardBuilder<TBuilder> : IFluentInterface where TBuilder : class
    {
        /// <summary>
        /// Returns this <see cref="IKeyboardBuilder{TBuilder}"/> as one marked for <see cref="InlineKeyboardMarkup"/> objects.
        /// </summary>
        TBuilder Inline(Action<IInlineKeyboardBuilder> buildAction);

        /// <summary>
        /// Returns this <see cref="IKeyboardBuilder{TBuilder}"/> as one marked for <see cref="ReplyKeyboardMarkup"/> objects.
        /// </summary>
        TBuilder Reply(Action<IReplyKeyboardBuilder> buildAction);

        /// <summary>
        /// Returns this <see cref="IKeyboardBuilder{TBuilder}"/> as one marked as <see cref="ReplyKeyboardRemove"/>.
        /// </summary>
        TBuilder Remove(bool selective = false);

        /// <summary>
        /// Returns this <see cref="IKeyboardBuilder{TBuilder}"/> as one marked as <see cref="ForceReplyMarkup"/>.
        /// </summary>
        TBuilder ForceReply(bool selective = false);
    }
}
