using System;
using System.Collections.Generic;
using System.Text;
using FluentCommands.Builders;
using FluentCommands.Interfaces.BaseBuilderOfModule;
using Telegram.Bot.Types.ReplyMarkups;

namespace FluentCommands.Interfaces.KeyboardBuilders
{
    /// <summary>
    /// Fluent interface for creating Keyboard Builders (for <see cref="InlineKeyboardBuilder"/> and <see cref="ReplyKeyboardBuilder"/>).
    /// </summary>
    public interface IKeyboardBuilderOfModule : IFluentInterface
    {
        /// <summary>
        /// Returns this <see cref="IKeyboardBuilder"/> as one marked for <see cref="InlineKeyboardMarkup"/> objects.
        /// </summary>
        ICommandBaseOfModuleKeyboard Inline(Action<IInlineKeyboardBuilder> buildAction);

        /// <summary>
        /// Returns this <see cref="IKeyboardBuilder"/> as one marked for <see cref="ReplyKeyboardMarkup"/> objects.
        /// </summary>
        ICommandBaseOfModuleKeyboard Reply(Action<IReplyKeyboardBuilder> buildAction);
    }
}
