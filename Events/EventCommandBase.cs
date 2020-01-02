using FluentCommands.Commands;
using FluentCommands.Exceptions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace FluentCommands.Events
{
    internal class EventCommandBase<TContext, TArgs> : CommandBase<TContext, TArgs> where TContext : ICommandContext<TArgs> where TArgs : EventArgs
    {
        internal EventCommandBase(CommandBaseBuilder commandBase, MethodInfo method, Type module) : base(commandBase, method, module)
        {
            if (commandBase.CommandType != CommandType.Event) throw new CommandOnBuildingException("There was an error building Commands (EventCommandBase id not have the correct CommandType). This exception should not happen; please report it as a bug to the FluentCommands github page.");

            // To implement in v1.1.
        }
    }
}
