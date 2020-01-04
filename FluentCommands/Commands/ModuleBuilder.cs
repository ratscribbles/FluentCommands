using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using FluentCommands.Exceptions;
using FluentCommands.Interfaces;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.Enums;
using FluentCommands.Commands;
using FluentCommands.Interfaces.KeyboardBuilders;
using FluentCommands.Logging;
using FluentCommands.Cache;
using Telegram.Bot;
using FluentCommands.Interfaces.MenuBuilders;
using FluentCommands.Interfaces.BaseBuilders;

namespace FluentCommands.Commands
{
    /// <summary>
    /// Builder that creates <see cref="CommandBaseBuilder"/> objects to assemble into commands of this Module.
    /// </summary>
    public sealed class ModuleBuilder : IFluentInterface
    {
        /// <summary>The Dictionary containing all <see cref="CommandBaseBuilder"/> objects for this Module. Used for the creation of <see cref="FluentCommands.Command"/> objects.</summary>
        private readonly Dictionary<string, CommandBaseBuilder> _moduleCommandBases = new Dictionary<string, CommandBaseBuilder>();

        /// <summary> Stores the module type when using alternate building form. </summary>
        internal Type TypeStorage { get; }
        /// <summary>The config object for this <see cref="ModuleBuilder"/>. Use <see cref="SetConfig(ModuleConfig)"/> to update this.</summary>
        internal ModuleConfigBuilder? ConfigBuilder { get; private set; }
        /// <summary>The internal <see cref="CommandBaseBuilder"/> dictionary for this <see cref="ModuleBuilder"/>. Readonly.</summary>
        internal IReadOnlyDictionary<string, CommandBaseBuilder> ModuleCommandBases => _moduleCommandBases;

        /// <summary>
        /// Indexer used primarily for the "build action" <see cref="Action"/> version of the fluent builder API.
        /// </summary>
        /// <param name="key">The name of the <see cref="FluentCommands.Command"/> to access in the internal dictionary.</param>
        /// <returns>Returns a <see cref="ICommandBaseOnBuilding"/> with the key as its name.</returns>
        public ICommandBaseOnBuilding this[string key]
        {
            get
            {
                if (key is null) throw new CommandOnBuildingException($"Command was null in module: {TypeStorage.FullName ?? "NULL"}");
                if(!_moduleCommandBases.ContainsKey(key)) _moduleCommandBases.TryAdd(key, new CommandBaseBuilder(key));
                return _moduleCommandBases[key];
            }
            set => _moduleCommandBases[key] = (CommandBaseBuilder)value;
        }

        /// <summary>
        /// Instantiates a new <see cref="ModuleBuilder"/> to create <see cref="CommandBaseBuilder"/> objects with.
        /// </summary>
        /// <param name="t">The class this ModuleBuilder is targeting.</param>
        internal ModuleBuilder(Type t) => TypeStorage = t;

        internal void SetConfig(ModuleConfigBuilder cfg) => ConfigBuilder = cfg;
        internal ModuleConfig BuildConfig() => ConfigBuilder!.BuildConfig();

        /// <summary>
        /// Creates a new command for this module.
        /// </summary>
        /// <param name="commandName">The name of this <see cref="FluentCommands.Command"/>.</param>
        /// <returns>Returns this <see cref="ModuleBuilder"/> as an <see cref="ICommandBaseBuilderOfModule"/>, beginning the build process.</returns>
        public ICommandBaseOnBuilding Command(string commandName)
        {
            if (commandName is null) throw new CommandOnBuildingException($"Command was null in module: {TypeStorage.FullName ?? "NULL"}");
            if (!_moduleCommandBases.ContainsKey(commandName)) _moduleCommandBases.TryAdd(commandName, new CommandBaseBuilder(commandName));
            return _moduleCommandBases[commandName];
        }
    }
}
