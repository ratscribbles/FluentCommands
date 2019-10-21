using System;
using System.Collections.Generic;
using System.Text;
using FluentCommands.Builders;
using FluentCommands.Interfaces.BaseBuilders;
using Telegram.Bot.Types.ReplyMarkups;

namespace FluentCommands.Interfaces.KeyboardBuilders
{
    /// <summary>
    /// Fluent interface for creating Keyboard Builders (for <see cref="InlineKeyboardBuilder"/> and <see cref="ReplyKeyboardBuilder"/>).
    /// </summary>
    public interface IKeyboardBuilder : IFluentInterface
    {
        /// <summary>
        /// Returns this <see cref="IKeyboardBuilder"/> as one marked for <see cref="InlineKeyboardMarkup"/> objects.
        /// </summary>
        ICommandBaseKeyboard Inline(Action<IInlineKeyboardBuilder> buildAction);

        /// <summary>
        /// Returns this <see cref="IKeyboardBuilder"/> as one marked for <see cref="ReplyKeyboardMarkup"/> objects.
        /// </summary>
        ICommandBaseKeyboard Reply(Action<IReplyKeyboardBuilder> buildAction);
    }
}
