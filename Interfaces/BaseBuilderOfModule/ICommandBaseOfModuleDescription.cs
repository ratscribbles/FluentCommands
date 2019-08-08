using System;
using System.Collections.Generic;
using System.Text;
using FluentCommands.Builders;
using Telegram.Bot.Types.ReplyMarkups;

namespace FluentCommands.Interfaces.BaseBuilderOfModule
{
    /// <summary>
    /// Fluent builder of <see cref="CommandModuleBuilder{TModule}"/> with <see cref="HasKeyboard(Action{KeyboardBuilder})"/> as the first immediate option.
    /// </summary>
    /// <typeparam name="TModule">The class that represents a Module for the <see cref="CommandService"/> to construct <see cref="Command"/> objects from.</typeparam>
    public interface ICommandBaseOfModuleDescription<TModule> : IFluentInterface where TModule : class
    {
        /// <summary>
        /// Constructs a <see cref="KeyboardBuilder"/> for this command, to become a Keyboard Markup for display.
        /// </summary>
        /// <param name="buildAction">Delegate that constructs a <see cref="KeyboardBuilder"/> for this future <see cref="Command"/>.</param>
        /// <returns>Returns this <see cref="CommandModuleBuilder{TModule}"/> as an <see cref="ICommandBaseOfModuleKeyboard{TModule}"/>, removing this option from the fluent builder.</returns>
        ICommandBaseOfModuleKeyboard<TModule> HasKeyboard(Action<KeyboardBuilder> buildAction);
        /// <summary>
        /// Adds an <see cref="InlineKeyboardBuilder"/> to this command. Will display after this command is called by a user.
        /// </summary>
        /// <param name="markup">The <see cref="InlineKeyboardMarkup"/></param>
        /// <returns>Returns this <see cref="CommandModuleBuilder{TModule}"/> as an <see cref="ICommandBaseOfModuleKeyboard{TModule}"/>, removing this option from the fluent builder.</returns>
        ICommandBaseOfModuleKeyboard<TModule> HasKeyboard(InlineKeyboardMarkup markup);
        /// <summary>
        /// Adds a <see cref="ReplyKeyboardBuilder"/> to this command. Will display after this command is called by a user.
        /// </summary>
        /// <param name="markup">The <see cref="ReplyKeyboardMarkup"/> being added to this command.</param>
        /// <returns>Returns this <see cref="CommandModuleBuilder{TModule}"/> as an <see cref="ICommandBaseOfModuleKeyboard{TModule}"/>, removing this option from the fluent builder.</returns>
        ICommandBaseOfModuleKeyboard<TModule> HasKeyboard(ReplyKeyboardMarkup markup);
        /// <summary>
        /// Adds an <see cref="IKeyboardButton"/> to this command.
        /// <para>Other commands' Keyboards can reference this command, and its button will be displayed where referenced.</para>
        /// </summary>
        /// <param name="button">The button to be added to this command.</param>
        /// <returns>Returns this <see cref="CommandModuleBuilder{TModule}"/> as an <see cref="ICommandBaseOfModuleCompletion{TModule}"/>, signalling the end of this command's construction.</returns>
        ICommandBaseOfModuleCompletion<TModule> HasKeyboardButton(IKeyboardButton button);
        /// <summary>
        /// Marks this command as complete, prompting you to build another command.
        /// <para>(If you meant to end the command building process, call <see cref="Done"/> instead!)</para>
        /// </summary>
        /// <returns>Returns this <see cref="CommandModuleBuilder{TModule}"/> as an <see cref="ICommandModuleBuilder{TModule}"/> to begin the command building process again.</returns>
        ICommandModuleBuilder<TModule> Next();
        /// <summary>
        /// Marks the entire module as complete so that <see cref="FluentCommands.Command"/> objects can be created.
        /// </summary>
        void Done();
    }
}
