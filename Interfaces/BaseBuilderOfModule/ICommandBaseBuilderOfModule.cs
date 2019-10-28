using System;
using System.Collections.Generic;
using System.Text;
using FluentCommands.Builders;
using FluentCommands.Interfaces.KeyboardBuilders;
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
        /// Adds a description to this command. Will show when "help" is called, unless default help command is disabled.
        /// </summary>
        /// <param name="description">The description of this command.</param>
        /// <returns>Returns this <see cref="ModuleBuilder"/> as an <see cref="ICommandBaseOfModuleDescriptionBuilder"/>, prompting the user to add a <see cref="Telegram.Bot.Types.Enums.ParseMode"/>.</returns>
        ICommandBaseOfModuleDescriptionBuilder HelpDescription(string description);
        /// <summary>
        /// Constructs a <see cref="KeyboardBuilder"/> for this command, to become a Keyboard Markup for display.
        /// </summary>
        IKeyboardBuilder<ICommandBaseOfModuleKeyboard> ReplyMarkup();
        /// <summary>
        /// Adds an <see cref="InlineKeyboardBuilder"/> to this command. Will display after this command is called by a user.
        /// </summary>
        /// <param name="markup">The <see cref="InlineKeyboardMarkup"/></param>
        /// <returns>Returns this <see cref="ModuleBuilder"/> as an <see cref="ICommandBaseOfModuleKeyboard"/>, removing this option from the fluent builder.</returns>
        ICommandBaseOfModuleKeyboard ReplyMarkup(InlineKeyboardMarkup markup);
        /// <summary>
        /// Adds a <see cref="ReplyKeyboardBuilder"/> to this command. Will display after this command is called by a user.
        /// </summary>
        /// <param name="markup">The <see cref="ReplyKeyboardMarkup"/> being added to this command.</param>
        /// <returns>Returns this <see cref="ModuleBuilder"/> as an <see cref="ICommandBaseOfModuleKeyboard"/>, removing this option from the fluent builder.</returns>
        ICommandBaseOfModuleKeyboard ReplyMarkup(ReplyKeyboardMarkup markup);
        /// <summary>
        /// Adds an <see cref="IKeyboardButton"/> to this command.
        /// <para>Other commands' Keyboards can reference this command, and its button will be displayed where referenced.</para>
        /// </summary>
        /// <param name="button">The button to be added to this command.</param>
        /// <returns>Returns this <see cref="ModuleBuilder"/> as an <see cref="ICommandBaseOfModuleCompletion"/>, signalling the end of this command's construction.</returns>
        ICommandBaseOfModuleCompletion KeyboardButtonReference(IKeyboardButton button);
        /// <summary>
        /// Marks this command as complete, prompting you to build another command.
        /// <para>(If you meant to end the command building process, call <see cref="Done"/> instead!)</para>
        /// </summary>
        /// <returns>Returns this <see cref="ModuleBuilder"/> as an <see cref="IModuleBuilder"/> to begin the command building process again.</returns>
        IModuleBuilder Next();
    }
}
