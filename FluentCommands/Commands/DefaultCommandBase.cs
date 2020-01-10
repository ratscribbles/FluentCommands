using FluentCommands.Exceptions;
using FluentCommands.Utility;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace FluentCommands.Commands
{
    internal class DefaultCommandBase<TContext, TArgs> : CommandBase, ICommand 
        where TContext : ICommandContext<TArgs> 
        where TArgs : EventArgs
    {
        internal CommandDelegate<TContext, TArgs> Invoke { get; }

        public DefaultCommandBase(MethodInfo method, CommandBaseBuilder commandBase, Type module) : base(commandBase, module)
        {
            if (CommandType != CommandType.Default) throw new CommandOnBuildingException("There was an error building Commands (DefaultCommandBase did not have the correct CommandType). This exception should not happen; please report it as a bug to the FluentCommands github page.");
            else
            {
                if (AuxiliaryMethods.TryConvertDelegate<TContext, TArgs>(method, out var c)) Invoke = c;
                else throw new ArgumentException();
            }
        }
    }
}
