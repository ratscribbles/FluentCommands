using FluentCommands.Commands;
using FluentCommands.Exceptions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace FluentCommands.Commands.Events
{
    internal class EventCommandBase<TContext, TArgs> : CommandBase /* IEventCommand */
        //: Consider constraining type on the EventType, or similar solution
        where TContext : ICommandContext<TArgs> 
        where TArgs : EventArgs
    {
        internal override CommandType CommandType => CommandType.Event;

        internal EventCommandBase(MethodInfo method, CommandBaseBuilder commandBase, Type module) : base(commandBase, module)
        {
            //: To implement in v1.1.
        }
    }
}
