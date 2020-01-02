using System;
using System.Collections.Generic;
using System.Text;
using FluentCommands.Interfaces.BaseBuilders;
using FluentCommands.Interfaces.KeyboardBuilders;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace FluentCommands.Interfaces
{
    /// <summary>
    /// Fluent interface responsible for easing the user through the <see cref="CommandBaseBuilder"/> construction process.
    /// </summary>
    public interface ICommandBaseBuilder : IFluentInterface
    {
        /// <summary>
        /// Adds aliases (alternate names) to this <see cref="ICommandBaseBuilder"/>.
        /// </summary>
        /// <param name="aliases">The alternate names for this future <see cref="Command"/>.</param>
        /// <returns>Returns this <see cref="ICommandBaseBuilder"/> as an <see cref="ICommandBaseAliases"/>, removing this option from the fluent builder.</returns>
        ICommandBaseAliases Aliases(params string[] aliases);
        /// <summary>
        /// Adds a description to this <see cref="ICommandBaseBuilder"/>.
        /// </summary>
        /// <param name="helpMessage">The <see cref="Menus.Menu"/> used when help is called on this <see cref="Command"/>.</param>
        /// <returns>Returns this <see cref="ICommandBaseBuilder"/> as an <see cref="ICommandBaseDescription"/>, prompting the user for a <see cref="Telegram.Bot.Types.Enums.ParseMode"/>.</returns>
        ICommandBaseDescription HelpDescription(Menus.Menu helpMessage);
        /// <summary>
        /// Adds a description to this <see cref="ICommandBaseBuilder"/>.
        /// </summary>
        /// <param name="description">The description of this future <see cref="Command"/>.</param>
        /// <returns>Returns this <see cref="ICommandBaseBuilder"/> as an <see cref="ICommandBaseDescription"/>, prompting the user for a <see cref="Telegram.Bot.Types.Enums.ParseMode"/>.</returns>
        ICommandBaseDescription HelpDescription(string description, ParseMode parseMode = ParseMode.Default);
        /// <summary>
        /// Adds an <see cref="InlineKeyboardButton"/> to this <see cref="ICommandBaseBuilder"/>. Ends fluent building for this object (this is the final option availalble).
        /// </summary>
        /// <param name="button">The <see cref="InlineKeyboardButton"/> for this future <see cref="Command"/>.</param>
        void InlineKeyboardButtonReference(InlineKeyboardButton button);
    }
}
