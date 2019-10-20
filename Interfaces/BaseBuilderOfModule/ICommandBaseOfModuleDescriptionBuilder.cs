using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types.Enums;

namespace FluentCommands.Interfaces.BaseBuilderOfModule
{
    /// <summary>
    /// Fluent builder of <see cref="Builders.ModuleBuilder"/> that prompts the user for a <see cref="ParseMode"/> to pair with the Description.
    /// </summary>
    public interface ICommandBaseOfModuleDescriptionBuilder : IFluentInterface
    {
        /// <summary>
        /// Adds a <see cref="ParseMode"/> to the description of this command.
        /// </summary>
        /// <param name="parseMode"></param>
        /// <returns>Returns this <see cref="Builders.ModuleBuilder"/> as an <see cref="ICommandBaseOfModuleDescription"/>, removing this option from the fluent builder.</returns>
        ICommandBaseOfModuleDescription WithParseMode(ParseMode parseMode);
    }
}
