using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FluentCommands.Commands
{
    internal delegate Task CommandDelegate<TContext, TArgs>(TContext e) where TContext : ICommandContext<TArgs> where TArgs : EventArgs;
    internal delegate Task<TReturn> CommandDelegate<TContext, TArgs, TReturn>(TContext e) where TContext : ICommandContext<TArgs> where TArgs : EventArgs;
}
