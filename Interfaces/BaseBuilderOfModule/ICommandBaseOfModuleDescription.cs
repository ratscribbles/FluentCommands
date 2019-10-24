﻿using System;
using System.Collections.Generic;
using System.Text;
using FluentCommands.Builders;
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
        /// Constructs a <see cref="KeyboardBuilder"/> for this command, to become a Keyboard Markup for display.
        /// </summary>
        IKeyboardBuilderOfModule Keyboard();
        /// <summary>
        /// Adds an <see cref="InlineKeyboardBuilder"/> to this command. Will display after this command is called by a user.
        /// </summary>
        /// <param name="markup">The <see cref="InlineKeyboardMarkup"/></param>
        /// <returns>Returns this <see cref="ModuleBuilder"/> as an <see cref="ICommandBaseOfModuleKeyboard"/>, removing this option from the fluent builder.</returns>
        ICommandBaseOfModuleKeyboard Keyboard(InlineKeyboardMarkup markup);
        /// <summary>
        /// Adds a <see cref="ReplyKeyboardBuilder"/> to this command. Will display after this command is called by a user.
        /// </summary>
        /// <param name="markup">The <see cref="ReplyKeyboardMarkup"/> being added to this command.</param>
        /// <returns>Returns this <see cref="ModuleBuilder"/> as an <see cref="ICommandBaseOfModuleKeyboard"/>, removing this option from the fluent builder.</returns>
        ICommandBaseOfModuleKeyboard Keyboard(ReplyKeyboardMarkup markup);
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
