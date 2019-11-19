using FluentCommands.Builders;
using FluentCommands.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace FluentCommands.Interfaces
{
    internal interface IReadOnlyModule
    {
        internal ModuleBuilderConfig Config { get; }
        internal IReadOnlyDictionary<string, CommandBaseBuilder> ModuleCommandBases { get; }
        internal IFluentLogger Logger { get; }
    }
}
