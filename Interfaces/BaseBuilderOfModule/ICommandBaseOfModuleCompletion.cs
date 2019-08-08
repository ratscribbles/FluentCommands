using System;
using System.Collections.Generic;
using System.Text;
using FluentCommands.Builders;
using Telegram.Bot.Types.ReplyMarkups;

namespace FluentCommands.Interfaces.BaseBuilderOfModule
{
    /// <summary>
    /// Fluent builder of <see cref="CommandModuleBuilder{TModule}"/> with only the final options available.
    /// </summary>
    /// <typeparam name="TModule">The class that represents a Module for the <see cref="CommandService"/> to construct <see cref="Command"/> objects from.</typeparam>
    public interface ICommandBaseOfModuleCompletion<TModule> : IFluentInterface where TModule : class
    {
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
