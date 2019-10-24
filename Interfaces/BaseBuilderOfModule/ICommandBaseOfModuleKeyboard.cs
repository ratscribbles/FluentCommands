using System;
using System.Collections.Generic;
using System.Text;
using FluentCommands.Builders;
using Telegram.Bot.Types.ReplyMarkups;

namespace FluentCommands.Interfaces.BaseBuilderOfModule
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICommandBaseOfModuleKeyboard : IFluentInterface
    {
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
