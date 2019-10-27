using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using FluentCommands.Exceptions;
using FluentCommands.Menus;

namespace FluentCommands
{
    public enum MenuMode
    {
        Default = 0,
        NoAction,
        EditLastMessage,
        EditOrDeleteLastMessage,
    }
    public class ModuleBuilderConfig
    {
        //? Consider moving away from an internal state handler and forcing the user to handle it on their own.
        //! Would conseqently force users to create their own solutions everytime (which would usually be about the same)
        public bool UseInternalKeyboardStateHandler { get; set; } = true;
        public bool UseDefaultErrorMessage { get; set; } = true;
        public bool BruteForceKeyboardReferences { get; set; } = false;
        public bool DeleteCommandAfterCall { get; set; } = false;
        public bool LogModuleActivities { get; set; } = false;
        public string Prefix { get; set; } = "/";
        public Menu DefaultErrorMessage { get; set; } = MenuItem.As().Text().TextSource("ERROR OCCURRED.").Done();
        public MenuMode MenuModeOverride { get; set; } = MenuMode.NoAction;

        //! Put this in the commandservice class

        /// <summary>
        /// Changes the prefix for this command module.
        /// <para><see cref="Command"/> module prefixes cannot be null or empty, be longer than 255 characters, or contain whitespace characters of any kind.</para>
        /// </summary>
        /// <exception cref="InvalidConfigSettingsException"></exception>
        /// <exception cref="RegexMatchTimeoutException"></exception>
        /// <param name="newPrefix"></param>
        public void ChangePrefix(string newPrefix)
        {
            if (string.IsNullOrWhiteSpace(newPrefix)) throw new InvalidConfigSettingsException("Command module prefix was null, empty, or only whitespace characters.");
            if (newPrefix.Length > 255) throw new InvalidConfigSettingsException("Command module prefixes may only be a maximum of 255 characters.");
            if (FluentRegex.CheckForWhiteSpaces.IsMatch(newPrefix)) throw new InvalidConfigSettingsException("Command module prefixes may not contain whitespace characters.");

            Prefix = newPrefix;
        }
    }
}
