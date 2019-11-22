﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FluentCommands.Builders;
using FluentCommands.Interfaces;
using FluentCommands.Logging;

namespace FluentCommands
{
    public abstract class CommandModule<TModule> where TModule : class
    {
        /// <summary>The class that contains the actual command implementations for this module.</summary>
        internal Type CommandClass { get; } = typeof(TModule);

        /// <summary>
        /// Builds a <see cref="Command"/> module.
        /// </summary>
        /// <param name="moduleBuilder"></param>
        protected abstract void OnBuilding(ModuleBuilder moduleBuilder);
        /// <summary>
        /// Sets the configuration for this <see cref="ModuleBuilder"/>.
        /// </summary>
        /// <param name="moduleBuilderConfig"></param>
        protected virtual void OnConfiguring(ModuleConfigBuilder config) { }

        private protected CommandModule() { }
        internal CommandModule(Action<ModuleBuilder> onBuilding) { }
    }
}
