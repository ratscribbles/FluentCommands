using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types.ReplyMarkups;
using FluentCommands.Builders;

namespace FluentCommands.Interfaces.BaseBuilders
{
    /// <summary>
    /// Fluent builder selection of <see cref="ICommandBaseBuilder{TModule}"/>, with <see cref="HasKeyboard(Action{KeyboardBuilder})"/> being the first available option in Intellisense.
    /// </summary>
    /// <typeparam name="TModule">The class that represents a Module for the <see cref="CommandService"/> to construct <see cref="Command"/> objects from.</typeparam>
    public interface ICommandBaseDescription<TModule> : IFluentInterface where TModule : class
    {
        /// <summary>
        /// Constructs a <see cref="KeyboardBuilder"/> for this <see cref="ICommandBaseBuilder{TModule}"/>.
        /// </summary>
        /// <param name="buildAction">Delegate that constructs a <see cref="KeyboardBuilder"/> for this future <see cref="Command"/>.</param>
        /// <returns>Returns this <see cref="ICommandBaseBuilder{TModule}"/> as an <see cref="ICommandBaseKeyboard{TModule}"/>, removing this option from the fluent builder.</returns>
        ICommandBaseKeyboard<TModule> HasKeyboard(Action<KeyboardBuilder> buildAction);
        /// <summary>
        /// Adds an <see cref="InlineKeyboardMarkup"/> to the <see cref="KeyboardBuilder"/> of this <see cref="ICommandBaseBuilder{TModule}"/>.
        /// </summary>
        /// <param name="markup">The <see cref="InlineKeyboardMarkup"/> for this future <see cref="Command"/>.</param>
        /// <returns>Returns this <see cref="ICommandBaseBuilder{TModule}"/> as an <see cref="ICommandBaseKeyboard{TModule}"/>, removing this option from the fluent builder.</returns>
        ICommandBaseKeyboard<TModule> HasKeyboard(InlineKeyboardMarkup markup);
        /// <summary>
        /// Adds a <see cref="ReplyKeyboardMarkup"/> to the <see cref="KeyboardBuilder"/> of this <see cref="ICommandBaseBuilder{TModule}"/>.
        /// </summary>
        /// <param name="markup">The <see cref="ReplyKeyboardMarkup"/> for this future <see cref="Command"/>.</param>
        /// <returns>Returns this <see cref="ICommandBaseBuilder{TModule}"/> as an <see cref="ICommandBaseKeyboard{TModule}"/>, removing this option from the fluent builder.</returns>
        ICommandBaseKeyboard<TModule> HasKeyboard(ReplyKeyboardMarkup markup);
        /// <summary>
        /// Adds an <see cref="IKeyboardButton"/> to this <see cref="ICommandBaseBuilder{TModule}"/>. Ends fluent building for this object (this is the final option availalble).
        /// </summary>
        /// <param name="button">The <see cref="IKeyboardButton"/> for this future <see cref="Command"/>.</param>
        void HasKeyboardButton(IKeyboardButton button);
    }
}
