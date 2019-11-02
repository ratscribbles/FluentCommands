using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using FluentCommands.Exceptions;
using FluentCommands.Interfaces;
using FluentCommands.Interfaces.BaseBuilderOfModule;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.Enums;
using FluentCommands.CommandTypes;
using FluentCommands.Interfaces.KeyboardBuilders;

namespace FluentCommands.Builders
{
    /// <summary>
    /// Builder that creates <see cref="CommandBaseBuilder"/> objects to assemble into commands of this Module.
    /// </summary>
    public sealed class ModuleBuilder : IModuleBuilder, 
        ICommandBaseBuilderOfModule, ICommandBaseOfModuleDescriptionBuilder, ICommandBaseOfModuleAliases, ICommandBaseOfModuleDescription, ICommandBaseOfModuleKeyboard, ICommandBaseOfModuleCompletion, IKeyboardBuilder<ICommandBaseOfModuleKeyboard>, IReplyMarkupable<ICommandBaseOfModuleKeyboard>,
        IFluentInterface
    {
        /// <summary> Stores the module type when using alternate building form. </summary>
        private Type TypeStorage { get; }
        /// <summary> Stores the name of the <see cref="CommandBaseBuilder"/> object. </summary>
        private CommandBaseBuilder? CommandStorage { get; set; } = null;
        /// <summary>The config object for this <see cref="ModuleBuilder"/>. Use <see cref="SetConfig(ModuleBuilderConfig)"/> to update this.</summary>
        internal ModuleBuilderConfig Config { get; private set; } = new ModuleBuilderConfig();
        /// <summary>The Dictionary containing all <see cref="CommandBaseBuilder"/> objects for this Module. Used for the creation of <see cref="FluentCommands.Command"/> objects.</summary>
        internal readonly Dictionary<string, CommandBaseBuilder> ModuleCommandBases = new Dictionary<string, CommandBaseBuilder>();

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
                if(!ModuleCommandBases.ContainsKey(key)) ModuleCommandBases.TryAdd(key, new CommandBaseBuilder(key));
                return ModuleCommandBases[key];
            }
            set => ModuleCommandBases[key] = (CommandBaseBuilder)value;
        }

        /// <summary>
        /// Instantiates a new <see cref="ModuleBuilder"/> to create <see cref="CommandBaseBuilder"/> objects with.
        /// </summary>
        /// <param name="t">The class this ModuleBuilder is targeting.</param>
        internal ModuleBuilder(Type t) => TypeStorage = t;

        internal void SetConfig(ModuleBuilderConfig cfg) => Config = cfg;

        /// <summary>
        /// Creates a new command for this module.
        /// </summary>
        /// <param name="commandName">The name of this <see cref="FluentCommands.Command"/>.</param>
        /// <returns>Returns this <see cref="ModuleBuilder"/> as an <see cref="ICommandBaseBuilderOfModule"/>, beginning the build process.</returns>
        public ICommandBaseBuilderOfModule Command(string commandName)
        {
            if (commandName is null) throw new CommandOnBuildingException($"Command name in module {TypeStorage.FullName ?? "??NULL??"} was null.");
            if (ModuleCommandBases.ContainsKey(commandName)) throw new DuplicateCommandException($"There was more than one command detected in module: {TypeStorage.FullName ?? "??NULL??"}, with the command name: \"{commandName}\"");
            CommandStorage = (CommandBaseBuilder)this[commandName];
            if(!(TypeStorage is null)) CommandService.Modules[TypeStorage] = this;
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
        /// <returns>Returns this <see cref="ModuleBuilder"/> as an <see cref="ICommandBaseOfModuleDescriptionBuilder"/>, prompting the user to add a <see cref="ParseMode"/>.</returns>
        public ICommandBaseOfModuleDescriptionBuilder HelpDescription(string description)
        {
            CommandStorage!.HelpDescription(description);
            UpdateBuilder(CommandStorage);
            return this;
        }
        /// <summary>
        /// Adds a <see cref="ParseMode"/> to the description of this command.
        /// </summary>
        /// <param name="parseMode"></param>
        /// <returns>Returns this <see cref="ModuleBuilder"/> as an <see cref="ICommandBaseOfModuleDescription"/>, removing this option from the fluent builder.</returns>
        public ICommandBaseOfModuleDescription UsingParseMode(ParseMode parseMode)
        {
            CommandStorage!.UsingParseMode(parseMode);
            UpdateBuilder(CommandStorage);
            return this;
        }
        /// <summary>
        /// Constructs a <see cref="KeyboardBuilder"/> for this command, to become a Keyboard Markup for display.
        /// </summary>
        /// <param name="buildAction">Delegate that constructs a <see cref="KeyboardBuilder"/> for this future <see cref="FluentCommands.Command"/>.</param>
        /// <returns>Returns this <see cref="ModuleBuilder"/> as an <see cref="ICommandBaseOfModuleKeyboard"/>, removing this option from the fluent builder.</returns>
        public IKeyboardBuilder<ICommandBaseOfModuleKeyboard> ReplyMarkup() => this;

        /// <summary>
        /// Returns this <see cref="IKeyboardBuilder"/> as one marked for <see cref="InlineKeyboardMarkup"/> objects.
        /// </summary>
        public ICommandBaseOfModuleKeyboard Inline(Action<IInlineKeyboardBuilder> buildAction)
        {
            CommandStorage!.ReplyMarkup().Inline(buildAction);
            UpdateBuilder(CommandStorage);
            return this;
        }
        /// <summary>
        /// Returns this <see cref="IKeyboardBuilder"/> as one marked for <see cref="ReplyKeyboardMarkup"/> objects.
        /// </summary>
        public ICommandBaseOfModuleKeyboard Reply(Action<IReplyKeyboardBuilder> buildAction)
        {
            CommandStorage!.ReplyMarkup().Reply(buildAction);
            UpdateBuilder(CommandStorage);
            return this;
        }
        /// <summary>
        /// Adds an <see cref="InlineKeyboardBuilder"/> to this command. Will display after this command is called by a user.
        /// </summary>
        /// <param name="markup">The <see cref="InlineKeyboardMarkup"/></param>
        /// <returns>Returns this <see cref="ModuleBuilder"/> as an <see cref="ICommandBaseOfModuleKeyboard"/>, removing this option from the fluent builder.</returns>
        public ICommandBaseOfModuleKeyboard ReplyMarkup(InlineKeyboardMarkup markup)
        {
            CommandStorage!.ReplyMarkup(markup);
            UpdateBuilder(CommandStorage);
            return this;
        }
        /// <summary>
        /// Adds a <see cref="ReplyKeyboardBuilder"/> to this command. Will display after this command is called by a user.
        /// </summary>
        /// <param name="markup">The <see cref="ReplyKeyboardMarkup"/> being added to this command.</param>
        /// <returns>Returns this <see cref="ModuleBuilder"/> as an <see cref="ICommandBaseOfModuleKeyboard"/>, removing this option from the fluent builder.</returns>
        public ICommandBaseOfModuleKeyboard ReplyMarkup(ReplyKeyboardMarkup markup)
        {
            CommandStorage!.ReplyMarkup(markup);
            UpdateBuilder(CommandStorage);
            return this;
        }
        public ICommandBaseOfModuleKeyboard ReplyMarkup(ForceReplyMarkup markup, bool selective = false)
        {
            CommandStorage!.ReplyMarkup(markup, selective);
            UpdateBuilder(CommandStorage);
            return this;
        }

        public ICommandBaseOfModuleKeyboard ReplyMarkup(ReplyKeyboardRemove markup, bool selective = false)
        {
            CommandStorage!.ReplyMarkup(markup, selective);
            UpdateBuilder(CommandStorage);
            return this;
        }
        /// <summary>
        /// Adds an <see cref="IKeyboardButton"/> to this command.
        /// <para>Other commands' Keyboards can reference this command, and its button will be displayed where referenced.</para>
        /// </summary>
        /// <param name="button">The button to be added to this command.</param>
        /// <returns>Returns this <see cref="ModuleBuilder"/> as an <see cref="ICommandBaseOfModuleCompletion"/>, signalling the end of this command's construction.</returns>
        public ICommandBaseOfModuleCompletion KeyboardButtonReference(IKeyboardButton button)
        {
            CommandStorage!.KeyboardButtonReference(button);
            UpdateBuilder(CommandStorage);
            return this;
        }
        /// <summary>
        /// Marks this command as complete, prompting you to build another command.
        /// <para>(If you meant to end the command building process, call <see cref="Done"/> instead!)</para>
        /// </summary>
        /// <returns>Returns this <see cref="ModuleBuilder"/> as an <see cref="IModuleBuilder"/> to begin the command building process again.</returns>
        public IModuleBuilder Next() => this;

        public ICommandBaseOfModuleKeyboard Remove(bool selective = false)
        {
            CommandStorage!.Remove(selective);
            UpdateBuilder(CommandStorage);
            return this;
        }

        public ICommandBaseOfModuleKeyboard ForceReply(bool selective = false)
        {
            CommandStorage!.ForceReply(selective);
            UpdateBuilder(CommandStorage);
            return this;
        }

        private void UpdateBuilder(CommandBaseBuilder c)
        {
            if (!(c is null)) this[c.Name] = c;
            if (!(TypeStorage is null)) CommandService.Modules[TypeStorage] = this;
        }
    }
}
