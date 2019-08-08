using System;
using System.Collections.Generic;
using System.Text;
using FluentCommands.Builders;
using FluentCommands.Interfaces.BaseBuilders;
using Telegram.Bot.Types.ReplyMarkups;

namespace FluentCommands.Interfaces
{
    /// <summary>
    /// Fluent interface responsible for easing the user through the <see cref="CommandBaseBuilder{TModule}"/> construction process.
    /// </summary>
    /// <typeparam name="TModule">The class that represents a Module for the <see cref="CommandService"/> to construct <see cref="Command"/> objects from.</typeparam>
    public interface ICommandBaseBuilder<TModule> : IFluentInterface where TModule : class
    {
        /// <summary>
        /// Adds aliases (alternate names) to this <see cref="ICommandBaseBuilder{TModule}"/>.
        /// </summary>
        /// <param name="aliases">The alternate names for this future <see cref="Command"/>.</param>
        /// <returns>Returns this <see cref="ICommandBaseBuilder{TModule}"/> as an <see cref="ICommandBaseAliases{TModule}"/>, removing this option from the fluent builder.</returns>
        ICommandBaseAliases<TModule> HasAliases(params string[] aliases);
        /// <summary>
        /// Adds a description to this <see cref="ICommandBaseBuilder{TModule}"/>.
        /// </summary>
        /// <param name="description">The description of this future <see cref="Command"/>.</param>
        /// <returns>Returns this <see cref="ICommandBaseBuilder{TModule}"/> as an <see cref="ICommandDescriptionBuilder{TModule}"/>, prompting the user for a <see cref="Telegram.Bot.Types.Enums.ParseMode"/>.</returns>
        ICommandDescriptionBuilder<TModule> HasHelpDescription(string description);
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
