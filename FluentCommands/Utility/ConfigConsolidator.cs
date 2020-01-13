using FluentCommands.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace FluentCommands.Utility
{
    internal class ConfigConsolidator
    {
        //: add properties here
        internal (int AmountOfMessages, TimeSpan PerTimeSpan) In_DefaultRateLimitPerUser { get; private set; }
        internal ConfigConsolidator(CommandServiceConfig commandServiceConfig, ModuleConfig moduleConfig)
        {

        }
    }
}
