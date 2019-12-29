using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types.ReplyMarkups;
using FluentCommands.Interfaces.KeyboardBuilders;
using Telegram.Bot.Types.Enums;

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
        /// <param name="helpMessage">The <see cref="Menus.Menu"/> used when help is called on this <see cref="Command"/>.</param>
        /// <returns>Returns this <see cref="ICommandBaseBuilder"/> as an <see cref="ICommandDescriptionBuilder"/>, prompting the user for a <see cref="Telegram.Bot.Types.Enums.ParseMode"/>.</returns>
        ICommandBaseDescription HelpDescription(Menus.Menu helpMessage);
        /// <summary>
        /// Adds a description to this <see cref="ICommandBaseBuilder"/>.
        /// </summary>
        /// <param name="description">The description of this future <see cref="Command"/>.</param>
        /// <returns>Returns this <see cref="ICommandBaseBuilder"/> as an <see cref="ICommandDescriptionBuilder"/>, prompting the user for a <see cref="Telegram.Bot.Types.Enums.ParseMode"/>.</returns>
        ICommandBaseDescription HelpDescription(string description, ParseMode parseMode = ParseMode.Default);
        /// <summary>
        /// Adds an <see cref="InlineKeyboardButton"/> to this <see cref="ICommandBaseBuilder"/>. Ends fluent building for this object (this is the final option availalble).
        /// </summary>
        /// <param name="button">The <see cref="InlineKeyboardButton"/> for this future <see cref="Command"/>.</param>
        void InlineKeyboardButtonReference(InlineKeyboardButton button);
    }
}
