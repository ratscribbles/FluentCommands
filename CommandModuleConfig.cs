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
        NoAction,
        EditLastMessage,
        EditOrDeleteLastMessage,
    }
    public class CommandModuleConfig
    {
        public bool UseInternalKeyboardStateHandler { get; private set; } = true;
        public bool UseDefaultErrorMessage { get; private set; } = true;
        public Menu DefaultErrorMessage { get; private set; } = MenuItem.As().Text().TextSource("ERROR OCCURRED.").Done();
        public string Prefix { get; private set; } = "/";
        public RegexOptions CommandNameRegexOptions { get; private set; } = RegexOptions.None;
        public bool DeleteCommandAfterCall { get; private set; } = false;
        public MenuMode MenuMode { get; private set; } = MenuMode.NoAction;

        /// <summary>
        /// Explicitly changes the prefix for this module.
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

        /// <summary>
        /// Explicitly changes the Default Error Message for this module.
        /// </summary>
        /// <param name="menuItem"></param>
        //public void ChangeDefaultErrorMessage(MenuItem menuItem) => DefaultErrorMessage = menuItem;
    }
}
