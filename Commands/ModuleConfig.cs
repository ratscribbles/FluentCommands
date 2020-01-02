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

namespace FluentCommands.Commands
{
    internal class ModuleConfig
    {
        private readonly ISendableMenu? _errorMsg;
        internal Type ModuleType { get; }
        internal ISendableMenu DefaultErrorMessageOverride
        {
            get
            {
                if (_errorMsg is null) return CommandService.GlobalConfig.CustomDefaultErrorMsg;
                else return _errorMsg;
            }
        }
        internal bool DeleteAllIncomingUserInputs { get; }
        internal bool DeleteCommandAfterCall { get; }
        internal bool DisableLogging { get; }
        internal FluentLogLevel MaximumLogLevelOverride { get; }
        internal MenuMode MenuModeOverride { get; }
        internal string Prefix { get; }
        internal (int AmountOfMessages, TimeSpan PerTimeSpan) RateLimitPerUser { get; }
        internal bool UsingBotClient { get; }
        internal bool UsingCustomCacheOverride { get; }
        internal bool UsingCustomLoggerOverride { get; }

        internal ModuleConfig(ModuleConfigBuilder b)
        {
            ModuleType = b.ModuleType;
            _errorMsg = b.In_DefaultErrorMessageOverride;
            DeleteAllIncomingUserInputs = b.In_DeleteAllIncomingUserInputs;
            DeleteCommandAfterCall = b.In_DeleteCommandAfterCall;
            DisableLogging = b.In_DisableLogging;
            MaximumLogLevelOverride = b.In_MaximumLogLevelOverride;
            MenuModeOverride = b.In_MenuModeOverride;
            Prefix = b.In_Prefix;
            RateLimitPerUser = b.In_RateLimitPerUser;
            UsingBotClient = b.In_UsingBotClient;
            UsingCustomCacheOverride = b.In_UsingCustomCacheOverride;
            UsingCustomLoggerOverride = b.In_UsingCustomLoggerOverride;
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

            //Prefix = newPrefix;

            //: Consider removing this; there's no way to guarantee command in-ambiguity if the user has the ability to change prefixes on the fly.
        }

    }
}
