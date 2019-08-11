using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using FluentCommands.Interfaces;
using FluentCommands.Interfaces.BaseBuilderOfModule;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.Enums;
using FluentCommands.CommandTypes;

namespace FluentCommands.Builders
{
    /// <summary>
    /// Builder that creates <see cref="CommandBaseBuilder{TModule}"/> objects to assemble into commands of this Module.
    /// </summary>
    /// <typeparam name="TModule">The class that represents a Module for the <see cref="CommandService"/> to construct <see cref="FluentCommands.Command"/> objects from.</typeparam>
    public sealed class CommandModuleBuilder<TModule> : ICommandModuleBuilder<TModule>, 
        ICommandBaseBuilderOfModule<TModule>, ICommandBaseOfModuleDescriptionBuilder<TModule>, ICommandBaseOfModuleAliases<TModule>, ICommandBaseOfModuleDescription<TModule>, ICommandBaseOfModuleKeyboard<TModule>, ICommandBaseOfModuleCompletion<TModule>,
        IFluentInterface where TModule : class
    {
        /// <summary>
        /// The Dictionary containing all <see cref="CommandBaseBuilder{TModule}"/> objects for this Module. Used for the creation of <see cref="CommandBase"/> and <see cref="FluentCommands.Command"/> objects.
        /// </summary>
        internal readonly Dictionary<string, CommandBaseBuilder<TModule>> BaseBuilderDictionary = new Dictionary<string, CommandBaseBuilder<TModule>>();
        /// <summary>
        /// Stores the name of the <see cref="CommandBaseBuilder{TModule}"/> object's Name 
        /// </summary>
        private CommandBaseBuilder<TModule> CommandStorage { get; set; }

        /// <summary>
        /// Indexer used primarily for the "build action" <see cref="Action"/> version of the fluent builder API.
        /// </summary>
        /// <param name="key">The name of the <see cref="FluentCommands.Command"/> to access in the internal dictionary.</param>
        /// <returns>Returns a <see cref="ICommandBaseBuilder{TModule}"/> with the key as its name.</returns>
        public ICommandBaseBuilder<TModule> this[string key]
        {
            get
            {
                if(!BaseBuilderDictionary.ContainsKey(key)) BaseBuilderDictionary.TryAdd(key, new CommandBaseBuilder<TModule>(key));
                return BaseBuilderDictionary[key];
            }
            set => BaseBuilderDictionary[key] = (CommandBaseBuilder<TModule>)value;
        }

        /// <summary>
        /// Instantiates a new <see cref="CommandModuleBuilder{TModule}"/> to create <see cref="CommandBaseBuilder{TModule}"/> objects with.
        /// </summary>
        internal CommandModuleBuilder() { }

        /// <summary>
        /// Creates a new command for this module.
        /// </summary>
        /// <param name="commandName">The name of this <see cref="FluentCommands.Command"/>.</param>
        /// <returns>Returns this <see cref="CommandModuleBuilder{TModule}"/> as an <see cref="ICommandBaseBuilderOfModule{TModule}"/>, beginning the build process.</returns>
        public ICommandBaseBuilderOfModule<TModule> Command(string commandName)
        {
            CommandStorage = (CommandBaseBuilder<TModule>)this[commandName];
            return this;
        }
        /// <summary>
        /// Adds aliases to this command.
        /// </summary>
        /// <param name="aliases">The aliases (alternate names) for this command.</param>
        /// <returns>Returns this <see cref="CommandModuleBuilder{TModule}"/> as an <see cref="ICommandBaseOfModuleAliases{TModule}"/>, removing this option from the fluent builder.</returns>
        public ICommandBaseOfModuleAliases<TModule> HasAliases(params string[] aliases)
        {
            CommandStorage.HasAliases(aliases);
            this[CommandStorage.Name] = CommandStorage;
            return this;
        }
        /// <summary>
        /// Adds a description to this command. Will show when "help" is called, unless default help command is disabled.
        /// </summary>
        /// <param name="description">The description of this command.</param>
        /// <returns>Returns this <see cref="CommandModuleBuilder{TModule}"/> as an <see cref="ICommandBaseOfModuleDescriptionBuilder{TModule}"/>, prompting the user to add a <see cref="ParseMode"/>.</returns>
        public ICommandBaseOfModuleDescriptionBuilder<TModule> HasDescription(string description)
        {
            CommandStorage.HasHelpDescription(description);
            this[CommandStorage.Name] = CommandStorage;
            return this;
        }
        /// <summary>
        /// Adds a <see cref="ParseMode"/> to the description of this command.
        /// </summary>
        /// <param name="parseMode"></param>
        /// <returns>Returns this <see cref="CommandModuleBuilder{TModule}"/> as an <see cref="ICommandBaseOfModuleDescription{TModule}"/>, removing this option from the fluent builder.</returns>
        public ICommandBaseOfModuleDescription<TModule> WithParseMode(ParseMode parseMode)
        {
            CommandStorage.WithParseMode(parseMode);
            this[CommandStorage.Name] = CommandStorage;
            return this;
        }
        /// <summary>
        /// Constructs a <see cref="KeyboardBuilder"/> for this command, to become a Keyboard Markup for display.
        /// </summary>
        /// <param name="buildAction">Delegate that constructs a <see cref="KeyboardBuilder"/> for this future <see cref="FluentCommands.Command"/>.</param>
        /// <returns>Returns this <see cref="CommandModuleBuilder{TModule}"/> as an <see cref="ICommandBaseOfModuleKeyboard{TModule}"/>, removing this option from the fluent builder.</returns>
        public ICommandBaseOfModuleKeyboard<TModule> HasKeyboard(Action<KeyboardBuilder> buildAction)
        {
            CommandStorage.HasKeyboard(buildAction);
            this[CommandStorage.Name] = CommandStorage;
            return this;
        }
        /// <summary>
        /// Adds an <see cref="InlineKeyboardBuilder"/> to this command. Will display after this command is called by a user.
        /// </summary>
        /// <param name="markup">The <see cref="InlineKeyboardMarkup"/></param>
        /// <returns>Returns this <see cref="CommandModuleBuilder{TModule}"/> as an <see cref="ICommandBaseOfModuleKeyboard{TModule}"/>, removing this option from the fluent builder.</returns>
        public ICommandBaseOfModuleKeyboard<TModule> HasKeyboard(InlineKeyboardMarkup markup)
        {
            CommandStorage.HasKeyboard(markup);
            this[CommandStorage.Name] = CommandStorage;
            return this;
        }
        /// <summary>
        /// Adds a <see cref="ReplyKeyboardBuilder"/> to this command. Will display after this command is called by a user.
        /// </summary>
        /// <param name="markup">The <see cref="ReplyKeyboardMarkup"/> being added to this command.</param>
        /// <returns>Returns this <see cref="CommandModuleBuilder{TModule}"/> as an <see cref="ICommandBaseOfModuleKeyboard{TModule}"/>, removing this option from the fluent builder.</returns>
        public ICommandBaseOfModuleKeyboard<TModule> HasKeyboard(ReplyKeyboardMarkup markup)
        {
            CommandStorage.HasKeyboard(markup);
            this[CommandStorage.Name] = CommandStorage;
            return this;
        }
        /// <summary>
        /// Adds an <see cref="IKeyboardButton"/> to this command.
        /// <para>Other commands' Keyboards can reference this command, and its button will be displayed where referenced.</para>
        /// </summary>
        /// <param name="button">The button to be added to this command.</param>
        /// <returns>Returns this <see cref="CommandModuleBuilder{TModule}"/> as an <see cref="ICommandBaseOfModuleCompletion{TModule}"/>, signalling the end of this command's construction.</returns>
        public ICommandBaseOfModuleCompletion<TModule> HasKeyboardButton(IKeyboardButton button)
        {
            CommandStorage.HasKeyboardButton(button);
            this[CommandStorage.Name] = CommandStorage;
            return this;
        }
        /// <summary>
        /// Marks this command as complete, prompting you to build another command.
        /// <para>(If you meant to end the command building process, call <see cref="Done"/> instead!)</para>
        /// </summary>
        /// <returns>Returns this <see cref="CommandModuleBuilder{TModule}"/> as an <see cref="ICommandModuleBuilder{TModule}"/> to begin the command building process again.</returns>
        public ICommandModuleBuilder<TModule> Next()
        {
            CommandStorage = null;
            return this;
        }
        /// <summary>
        /// Marks the entire module as complete so that <see cref="FluentCommands.Command"/> objects can be created.
        /// </summary>
        public void Done()
        {
            if(!CommandService.Modules.Contains(typeof(TModule))) CommandService.Modules.Add(typeof(TModule));
            if(!CommandService.RawCommands.ContainsKey(typeof(TModule))) CommandService.RawCommands.TryAdd(typeof(TModule), new List<CommandBase>());

            foreach (var item in BaseBuilderDictionary)
            {
                var thisBase = item.Value.ConvertToBase();
                CommandService.RawCommands[thisBase.Module].Add(thisBase);
            }
        }
    }
}
