﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using FluentCommands.Exceptions;
using FluentCommands.Interfaces;
using FluentCommands.Interfaces.BaseBuilderOfModule;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.Enums;
using FluentCommands.Commands;
using FluentCommands.Interfaces.KeyboardBuilders;
using FluentCommands.Logging;
using FluentCommands.Cache;
using Telegram.Bot;

namespace FluentCommands.Commands
{
    /// <summary>
    /// Builder that creates <see cref="CommandBaseBuilder"/> objects to assemble into commands of this Module.
    /// </summary>
    public sealed class ModuleBuilder : IModuleBuilder,
        ICommandBaseBuilderOfModule, ICommandBaseOfModuleAliases, ICommandBaseOfModuleDescription, ICommandBaseOfModuleCompletion,
        IFluentInterface
    {
        /// <summary>The Dictionary containing all <see cref="CommandBaseBuilder"/> objects for this Module. Used for the creation of <see cref="FluentCommands.Command"/> objects.</summary>
        private readonly Dictionary<string, CommandBaseBuilder> _moduleCommandBases = new Dictionary<string, CommandBaseBuilder>();

        /// <summary> Stores the module type when using alternate building form. </summary>
        internal Type TypeStorage { get; }
        /// <summary> Stores the name of the <see cref="CommandBaseBuilder"/> object. </summary>
        private CommandBaseBuilder? CommandStorage { get; set; }
        /// <summary>The config object for this <see cref="ModuleBuilder"/>. Use <see cref="SetConfig(ModuleConfig)"/> to update this.</summary>
        internal ModuleConfigBuilder? ConfigBuilder { get; private set; }
        /// <summary>The internal <see cref="CommandBaseBuilder"/> dictionary for this <see cref="ModuleBuilder"/>. Readonly.</summary>
        internal IReadOnlyDictionary<string, CommandBaseBuilder> ModuleCommandBases { get { return _moduleCommandBases; } }

        /// <summary>
        /// Indexer used primarily for the "build action" <see cref="Action"/> version of the fluent builder API.
        /// </summary>
        /// <param name="key">The name of the <see cref="FluentCommands.Command"/> to access in the internal dictionary.</param>
        /// <returns>Returns a <see cref="ICommandBaseBuilder"/> with the key as its name.</returns>
        public ICommandBaseBuilder this[string key]
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

        #region Builder Implementation
        /// <summary>
        /// Creates a new command for this module.
        /// </summary>
        /// <param name="commandName">The name of this <see cref="FluentCommands.Command"/>.</param>
        /// <returns>Returns this <see cref="ModuleBuilder"/> as an <see cref="ICommandBaseBuilderOfModule"/>, beginning the build process.</returns>
        public ICommandBaseBuilderOfModule Command(string commandName)
        {
            if (commandName is null) throw new CommandOnBuildingException($"Command name in module {TypeStorage.FullName ?? "??NULL??"} was null.");
            if (_moduleCommandBases.ContainsKey(commandName)) throw new DuplicateCommandException($"There was more than one command detected in module: {TypeStorage.FullName ?? "??NULL??"}, with the command name: \"{commandName}\"");
            CommandStorage = (CommandBaseBuilder)this[commandName];
            UpdateBuilder(CommandStorage); //: Double check this line if anything goes wrong.
            return this;
        }
        /// <summary>
        /// Adds aliases to this command. Duplicates are ignored.
        /// </summary>
        /// <param name="aliases">The aliases (alternate names) for this command.</param>
        /// <returns>Returns this <see cref="ModuleBuilder"/> as an <see cref="ICommandBaseOfModuleAliases"/>, removing this option from the fluent builder.</returns>
        public ICommandBaseOfModuleAliases Aliases(params string[] aliases)
        {
            if (aliases.Any(alias => alias is null)) throw new CommandOnBuildingException($"Command \"{CommandStorage?.Name ?? "??NULL??"}\" in module {TypeStorage?.FullName ?? "??NULL??"} had an alias that was null.");
            CommandStorage!.Aliases(aliases.Distinct().ToArray());
            UpdateBuilder(CommandStorage);
            return this;
        }
        /// <summary>
        /// Adds a description to this command. Will show when "help" is called, unless default help command is disabled.
        /// </summary>
        /// <param name="description">The description of this command.</param>
        /// <returns>Returns this <see cref="ModuleBuilder"/> as an <see cref="ICommandBaseOfModuleDescription"/>, prompting the user to add a <see cref="ParseMode"/>.</returns>
        public ICommandBaseOfModuleDescription HelpDescription(string description, ParseMode parseMode = ParseMode.Default)
        {
            CommandStorage!.HelpDescription(description);
            UpdateBuilder(CommandStorage);
            return this;
        }
        /// <summary>
        /// Adds a description to this command. Will show when "help" is called, unless default help command is disabled.
        /// </summary>
        /// <param name="description">The description of this command.</param>
        /// <returns>Returns this <see cref="ModuleBuilder"/> as an <see cref="ICommandBaseOfModuleDescription"/>, prompting the user to add a <see cref="ParseMode"/>.</returns>
        public ICommandBaseOfModuleDescription HelpDescription(Menus.Menu helpMessage)
        {
            CommandStorage!.HelpDescription(helpMessage);
            UpdateBuilder(CommandStorage);
            return this;
        }
        /// <summary>
        /// Adds an <see cref="IKeyboardButton"/> to this command.
        /// <para>Other commands' Keyboards can reference this command, and its button will be displayed where referenced.</para>
        /// </summary>
        /// <param name="button">The button to be added to this command.</param>
        /// <returns>Returns this <see cref="ModuleBuilder"/> as an <see cref="ICommandBaseOfModuleCompletion"/>, signalling the end of this command's construction.</returns>
        public ICommandBaseOfModuleCompletion InlineKeyboardButtonReference(InlineKeyboardButton button)
        {
            CommandStorage!.InlineKeyboardButtonReference(button);
            UpdateBuilder(CommandStorage);
            return this;
        }
        /// <summary>
        /// Marks this command as complete, prompting you to build another command.
        /// <para>(If you meant to end the command building process, call <see cref="Done"/> instead!)</para>
        /// </summary>
        /// <returns>Returns this <see cref="ModuleBuilder"/> as an <see cref="IModuleBuilder"/> to begin the command building process again.</returns>
        public IModuleBuilder Next() => this;

        /// <summary>Updates the <see cref="ModuleBuilder"/> in the Modules dictionary contained within the <see cref="CommandService"/>.</summary>
        /// <param name="c"></param>
        private void UpdateBuilder(CommandBaseBuilder c)
        {
            if (!(c is null)) this[c.Name] = c;
            if (!(TypeStorage is null)) CommandService.UpdateBuilderInTempModules(this, TypeStorage);
        }
        #endregion
    }
}