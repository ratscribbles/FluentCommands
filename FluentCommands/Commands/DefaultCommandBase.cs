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
            if (AuxiliaryMethods.TryConvertDelegate<TContext, TArgs>(method, out var c)) Invoke = c;
            else throw new ArgumentException();
        }
    }
}
