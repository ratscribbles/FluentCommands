using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types.ReplyMarkups;
using FluentCommands.Builders;
using FluentCommands.Interfaces.KeyboardBuilders;

namespace FluentCommands.Interfaces.BaseBuilders
{
    /// <summary>
    /// Fluent builder selection of <see cref="ICommandBaseBuilder"/>, with <see cref="HelpDescription(string)"/> being the first available option in Intellisense.
    /// </summary>
    public interface ICommandBaseAliases : IFluentInterface
    {
        /// <summary>
        /// Adds a description to this <see cref="ICommandBaseBuilder"/>.
        /// </summary>
        /// <param name="description">The description of this future <see cref="Command"/>.</param>
        /// <returns>Returns this <see cref="ICommandBaseBuilder"/> as an <see cref="ICommandDescriptionBuilder"/>, prompting the user for a <see cref="Telegram.Bot.Types.Enums.ParseMode"/>.</returns>
        ICommandDescriptionBuilder HelpDescription(string description);
        /// <summary>
        /// Constructs an <see cref="IKeyboardBuilder"/> for this <see cref="ICommandBaseBuilder"/>.
        /// </summary>
        /// <returns>Returns this <see cref="ICommandBaseBuilder"/> as an <see cref="ICommandBaseKeyboard"/>, removing this option from the fluent builder.</returns>
        IKeyboardBuilder<ICommandBaseKeyboard> ReplyMarkup();
        /// <summary>
        /// Adds an <see cref="InlineKeyboardMarkup"/> to the <see cref="KeyboardBuilder"/> of this <see cref="ICommandBaseBuilder"/>.
        /// </summary>
        /// <param name="markup">The <see cref="InlineKeyboardMarkup"/> for this future <see cref="Command"/>.</param>
        /// <returns>Returns this <see cref="ICommandBaseBuilder"/> as an <see cref="ICommandBaseKeyboard"/>, removing this option from the fluent builder.</returns>
        ICommandBaseKeyboard ReplyMarkup(InlineKeyboardMarkup markup);
        /// <summary>
        /// Adds a <see cref="ReplyKeyboardMarkup"/> to the <see cref="KeyboardBuilder"/> of this <see cref="ICommandBaseBuilder"/>.
        /// </summary>
        /// <param name="markup">The <see cref="ReplyKeyboardMarkup"/> for this future <see cref="Command"/>.</param>
        /// <returns>Returns this <see cref="ICommandBaseBuilder"/> as an <see cref="ICommandBaseKeyboard"/>, removing this option from the fluent builder.</returns>
        ICommandBaseKeyboard ReplyMarkup(ReplyKeyboardMarkup markup);
        /// <summary>
        /// Adds an <see cref="IKeyboardButton"/> to this <see cref="ICommandBaseBuilder"/>. Ends fluent building for this object (this is the final option availalble).
        /// </summary>
        /// <param name="button">The <see cref="IKeyboardButton"/> for this future <see cref="Command"/>.</param>
        void KeyboardButtonReference(IKeyboardButton button);
    }
}
