using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using FluentCommands.Exceptions;
using FluentCommands.Logging;
using FluentCommands.Menus;
using FluentCommands.Utility;
using FluentCommands.Cache;
using FluentCommands.Interfaces.MenuBuilders;

namespace FluentCommands
{
    internal class ModuleConfig
    {
        internal bool UseInternalKeyboardStateHandler { get; }
        internal bool UseDefaultErrorMessage { get; }
        internal bool BruteForceKeyboardReferences { get; }
        internal bool DeleteCommandAfterCall { get; }
        internal bool LogModuleActivities { get; }
        internal FluentLogLevel MaximumLogLevel { get; }
        internal string Prefix { get; private set; } = "/";
        internal int PerUserRateLimitOverride { get; private set; }
        internal IMenu DefaultErrorMessage { get; } = Menu.Text("ERROR!!! LMAO"); //: Modify this to be more professional. Later.
        internal MenuMode? MenuModeOverride { get; }

        internal ModuleConfig() { }
        internal ModuleConfig(ModuleConfigBuilder b)
        {
            BruteForceKeyboardReferences = b.BruteForceKeyboardReferences;
            DeleteCommandAfterCall = b.DeleteCommandAfterCall;
            LogModuleActivities = b.LogModuleActivities;
            MaximumLogLevel = b.MaximumLogLevelOverride;
            MenuModeOverride = b.MenuModeOverride;
            Prefix = b.Prefix;
            UseDefaultErrorMessage = b.UseDefaultErrorMessage;
            UseInternalKeyboardStateHandler = b.UseInternalKeyboardStateHandler;
        }

        //: Put this in the commandservice class as a generic method; have it seek the actual module and then change its prefix here

        /// <summary>
        /// Changes the prefix for this command module.
        /// <para><see cref="Command"/> module prefixes cannot be null or empty, be longer than 255 characters, or contain whitespace characters of any kind.</para>
        /// </summary>
        /// <exception cref="InvalidConfigSettingsException"></exception>
        /// <exception cref="RegexMatchTimeoutException"></exception>
        /// <param name="newPrefix"></param>
        internal void ChangePrefix(string newPrefix)
        {
            if (string.IsNullOrWhiteSpace(newPrefix)) throw new InvalidConfigSettingsException("Command module prefix was null, empty, or only whitespace characters.");
            if (newPrefix.Length > 255) throw new InvalidConfigSettingsException("Command module prefixes may only be a maximum of 255 characters.");
            if (FluentRegex.CheckForWhiteSpaces.IsMatch(newPrefix)) throw new InvalidConfigSettingsException("Command module prefixes may not contain whitespace characters.");

            Prefix = newPrefix;
        }

    }
}
