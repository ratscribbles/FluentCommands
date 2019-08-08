﻿using System;
using System.Collections.Generic;
using System.Text;
using FluentCommands.Interfaces.BaseBuilderOfModule;

namespace FluentCommands.Interfaces
{
    /// <summary>
    /// Fluent builder that creates <see cref="ICommandBaseBuilder{TModule}"/> objects to assemble into commands of this Module.
    /// </summary>
    /// <typeparam name="TModule">The class that represents a Module for the <see cref="CommandService"/> to construct <see cref="Command"/> objects from.</typeparam>
    public interface ICommandModuleBuilder<TModule> : IFluentInterface where TModule : class
    {
        /// <summary>
        /// Indexer used primarily for the "build action" <see cref="Action"/> version of the fluent builder API.
        /// </summary>
        /// <param name="key">The name of the <see cref="FluentCommands.Command"/> to access in the internal dictionary.</param>
        /// <returns>Returns a <see cref="ICommandBaseBuilder{TModule}"/> with the key as its name.</returns>
        ICommandBaseBuilder<TModule> this[string key] { get; set; }

        /// <summary>
        /// Creates a new command for this module.
        /// </summary>
        /// <param name="commandName">The name of this <see cref="FluentCommands.Command"/>.</param>
        /// <returns>Returns this <see cref="Builders.CommandModuleBuilder{TModule}"/> as an <see cref="ICommandBaseBuilderOfModule{TModule}"/>, beginning the build process.</returns>
        ICommandBaseBuilderOfModule<TModule> Command(string commandName);
    }
}
