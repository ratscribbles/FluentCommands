using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types.Enums;
using FluentCommands.Interfaces.BaseBuilders;

namespace FluentCommands.Interfaces
{
    /// <summary>
    /// Fluent builder selection of <see cref="ICommandBaseBuilder{TModule}"/>, with <see cref="WithParseMode(ParseMode)"/> being the only option available in Intellisense.
    /// <para>This interface is responsible for making sure the <see cref="Command.Description"/> has a <see cref="ParseMode"/> when sent to the user.</para>
    /// </summary>
    /// <typeparam name="TModule">The class that represents a Module for the <see cref="CommandService"/> to construct <see cref="Command"/> objects from.</typeparam>
    public interface ICommandDescriptionBuilder<TModule> : IFluentInterface where TModule : class
    {
        /// <summary>
        /// Adds the <see cref="Telegram.Bot.Types.Enums.ParseMode"/> for the description of this <see cref="ICommandBaseBuilder{TModule}"/>.
        /// </summary>
        /// <param name="parseMode">The <see cref="Telegram.Bot.Types.Enums.ParseMode"/> for the description of this future <see cref="Command"/>.</param>
        /// <returns>Returns this <see cref="ICommandBaseBuilder{TModule}"/> as an <see cref="ICommandBaseDescription{TModule}"/>, removing this option from the fluent builder.</returns>
        ICommandBaseDescription<TModule> WithParseMode(ParseMode parseMode);
    }
}
