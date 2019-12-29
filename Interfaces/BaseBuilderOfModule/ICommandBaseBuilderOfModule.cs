using System;
using System.Collections.Generic;
using System.Text;
using FluentCommands.Interfaces.KeyboardBuilders;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace FluentCommands.Interfaces.BaseBuilderOfModule
{
    /// <summary>
    /// Fluent builder for the alternate builder format of <see cref="ModuleBuilder"/>.
    /// </summary>
    public interface ICommandBaseBuilderOfModule : IFluentInterface
    {
        /// <summary>
        /// Adds aliases to this command.
        /// </summary>
        /// <param name="aliases">The aliases (alternate names) for this command.</param>
        /// <returns>Returns this <see cref="ModuleBuilder"/> as an <see cref="ICommandBaseOfModuleAliases"/>, removing this option from the fluent builder.</returns>
        ICommandBaseOfModuleAliases Aliases(params string[] aliases);
        /// <summary>
        /// Adds a description to this <see cref="ICommandBaseBuilder"/>. Will show when "help" is called, unless default help command is disabled.
        /// </summary>
        /// <param name="helpMessage">The <see cref="Menus.Menu"/> used when help is called on this <see cref="Command"/>.</param>
        /// <returns>Returns this <see cref="ICommandBaseBuilder"/> as an <see cref="ICommandDescriptionBuilder"/>, prompting the user for a <see cref="Telegram.Bot.Types.Enums.ParseMode"/>.</returns>
        ICommandBaseOfModuleDescription HelpDescription(Menus.Menu helpMessage);
        /// <summary>
        /// Adds a description to this <see cref="ICommandBaseBuilder"/>. Will show when "help" is called, unless default help command is disabled.
        /// </summary>
        /// <param name="description">The description of this future <see cref="Command"/>.</param>
        /// <returns>Returns this <see cref="ICommandBaseBuilder"/> as an <see cref="ICommandDescriptionBuilder"/>, prompting the user for a <see cref="Telegram.Bot.Types.Enums.ParseMode"/>.</returns>
        ICommandBaseOfModuleDescription HelpDescription(string description, ParseMode parseMode = ParseMode.Default);
        /// <summary>
        /// Adds an <see cref="InlineKeyboardButton"/> to this command.
        /// <para>Other commands' Keyboards can reference this command, and its button will be displayed where referenced.</para>
        /// </summary>
        /// <param name="button">The button to be added to this command.</param>
        /// <returns>Returns this <see cref="ModuleBuilder"/> as an <see cref="ICommandBaseOfModuleCompletion"/>, signalling the end of this command's construction.</returns>
        ICommandBaseOfModuleCompletion InlineKeyboardButtonReference(InlineKeyboardButton button);
        /// <summary>
        /// Marks this command as complete, prompting you to build another command.
        /// <para>(If you meant to end the command building process, call <see cref="Done"/> instead!)</para>
        /// </summary>
        /// <returns>Returns this <see cref="ModuleBuilder"/> as an <see cref="IModuleBuilder"/> to begin the command building process again.</returns>
        IModuleBuilder Next();
    }
}
