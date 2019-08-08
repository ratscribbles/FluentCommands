using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types.ReplyMarkups;
using FluentCommands.Builders;

namespace FluentCommands.Interfaces.BaseBuilders
{
    /// <summary>
    /// Fluent builder selection of <see cref="ICommandBaseBuilder{TModule}"/>, with <see cref="HasKeyboardButton(IKeyboardButton)"/> being the only available option in Intellisense.
    /// <para>This is the final builder option for <see cref="ICommandBaseBuilder{TModule}"/> objects.</para>
    /// </summary>
    /// <typeparam name="TModule">The class that represents a Module for the <see cref="CommandService"/> to construct <see cref="Command"/> objects from.</typeparam>
    public interface ICommandBaseKeyboard<TModule> : IFluentInterface where TModule : class
    {
        /// <summary>
        /// Adds an <see cref="IKeyboardButton"/> to this <see cref="ICommandBaseBuilder{TModule}"/>. Ends fluent building for this object (this is the final option availalble).
        /// </summary>
        /// <param name="button">The <see cref="IKeyboardButton"/> for this future <see cref="Command"/>.</param>
        void HasKeyboardButton(IKeyboardButton button);
    }
}
