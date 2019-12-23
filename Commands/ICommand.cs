using FluentCommands.Commands.Steps;
using System;
using System.Collections.Generic;
using System.Text;

namespace FluentCommands.Commands
{
    /// <summary>
    /// Marks an object as a valid <see cref="ICommand"/>. This interface is a marker for use in collections.
    /// </summary>
    internal interface ICommand
    {
        internal Type Module { get; }
        internal Type Args { get; }
        internal string Name { get; }
        internal CommandType CommandType { get; }
        internal StepContainer? StepInfo { get; }
    }
}
