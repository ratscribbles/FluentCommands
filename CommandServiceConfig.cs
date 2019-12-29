using FluentCommands.Cache;
using FluentCommands.Interfaces.MenuBuilders;
using FluentCommands.Logging;
using FluentCommands.Menus;
using System;
using System.Collections.Generic;
using System.Text;

namespace FluentCommands
{
    internal class CommandServiceConfig
    {
        internal bool BruteForceKeyboardReferences { get; }
        internal ISendableMenu? CustomDefaultErrorMsg { get; }
        internal ISendableMenu? CustomDefaultHelpMsg { get; }
        internal MenuMode DefaultMenuMode { get; }
        internal bool DisableLogging { get; }
        internal bool EnableManualConfiguration { get; }
        internal FluentLogLevel MaximumLogLevel { get; }
        internal (int AmountOfMessages, TimeSpan PerTimeSpan) DefaultRateLimitPerUser { get; } 
        internal bool SwallowCriticalExceptions { get; }
        internal bool UsingCustomCache { get; }
        internal bool UsingCustomDefaultHelpMsg { get; }
        internal bool UsingCustomDefaultErrorMsg { get; }
        internal bool UsingCustomLogger { get; }

        internal CommandServiceConfig(CommandServiceConfigBuilder c)
        {
            BruteForceKeyboardReferences = c.In_BruteForceKeyboardReferences;
            CustomDefaultErrorMsg = c.In_CustomDefaultErrorMsg;
            CustomDefaultHelpMsg = c.In_CustomDefaultHelpMsg;
            DefaultMenuMode = c.In_DefaultMenuMode;
            DisableLogging = c.In_DisableLoggingGlobally;
            EnableManualConfiguration = c.In_EnableManualConfiguration;
            MaximumLogLevel = c.In_MaximumLogLevel;
            DefaultRateLimitPerUser = c.In_DefaultRateLimitPerUser;
            SwallowCriticalExceptions = c.In_SwallowCriticalExceptions;
            UsingCustomCache = c.In_UsingCustomCache;
            UsingCustomDefaultErrorMsg = c.In_UsingCustomDefaultErrorMsg;
            UsingCustomDefaultHelpMsg = c.In_UsingCustomDefaultHelpMsg;
            UsingCustomLogger = c.In_UsingCustomLogger;
        }
    }
}
