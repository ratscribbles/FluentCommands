using FluentCommands.Builders;
using FluentCommands.Cache;
using FluentCommands.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace FluentCommands.Interfaces
{
    internal interface IReadOnlyModule
    {
        internal ModuleConfig Config { get; }
        internal IFluentLogger Logger { get; }
        internal Type TypeStorage { get; }
        internal IFluentDatabase Database { get; }
    }
}
