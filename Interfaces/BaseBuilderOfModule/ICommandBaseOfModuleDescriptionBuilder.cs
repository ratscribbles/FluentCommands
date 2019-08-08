using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types.Enums;

namespace FluentCommands.Interfaces.BaseBuilderOfModule
{
    /// <summary>
    /// Fluent builder of <see cref="Builders.CommandModuleBuilder{TModule}"/> that prompts the user for a <see cref="ParseMode"/> to pair with the Description.
    /// </summary>
    /// <typeparam name="TModule">The class that represents a Module for the <see cref="CommandService"/> to construct <see cref="Command"/> objects from.</typeparam>
    public interface ICommandBaseOfModuleDescriptionBuilder<TModule> : IFluentInterface where TModule : class
    {
        /// <summary>
        /// Adds a <see cref="ParseMode"/> to the description of this command.
        /// </summary>
        /// <param name="parseMode"></param>
        /// <returns>Returns this <see cref="Builders.CommandModuleBuilder{TModule}"/> as an <see cref="ICommandBaseOfModuleDescription{TModule}"/>, removing this option from the fluent builder.</returns>
        ICommandBaseOfModuleDescription<TModule> WithParseMode(ParseMode parseMode);
    }
}
