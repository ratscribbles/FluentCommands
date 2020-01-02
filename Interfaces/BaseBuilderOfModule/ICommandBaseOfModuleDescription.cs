using System;
using System.Collections.Generic;
using System.Text;
using FluentCommands.Interfaces.KeyboardBuilders;
using Telegram.Bot.Types.ReplyMarkups;

namespace FluentCommands.Interfaces.BaseBuilderOfModule
{
    /// <summary>
    /// Fluent builder of <see cref="ModuleBuilder"/> with <see cref="HasKeyboard(Action{KeyboardBuilder})"/> as the first immediate option.
    /// </summary>
    public interface ICommandBaseOfModuleDescription : IFluentInterface
    {
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
        /// <returns>Returns this <see cref="ModuleBuilder"/> as an <see cref="ICommandModuleBuilder"/> to begin the command building process again.</returns>
        ICommandModuleBuilder Next();
    }
}
