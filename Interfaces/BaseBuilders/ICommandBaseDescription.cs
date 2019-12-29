using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types.ReplyMarkups;
using FluentCommands.Interfaces.KeyboardBuilders;

namespace FluentCommands.Interfaces.BaseBuilders
{
    /// <summary>
    /// Fluent builder selection of <see cref="ICommandBaseBuilder"/>, with <see cref="HasKeyboard(Action{KeyboardBuilder})"/> being the first available option in Intellisense.
    /// </summary>
    public interface ICommandBaseDescription : IFluentInterface
    {
        /// <summary>
        /// Adds an <see cref="InlineKeyboardButton"/> to this <see cref="ICommandBaseBuilder"/>. Ends fluent building for this object (this is the final option availalble).
        /// </summary>
        /// <param name="button">The <see cref="InlineKeyboardButton"/> for this future <see cref="Command"/>.</param>
        void InlineKeyboardButtonReference(InlineKeyboardButton button);
    }
}
