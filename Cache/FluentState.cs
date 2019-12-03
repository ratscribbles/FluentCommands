using System;
using System.Collections.Generic;
using System.Text;

namespace FluentCommands.Cache
{
    public abstract class FluentState
    {
        /// <summary>Determines whether this state is in its default state.</summary>
        public virtual bool IsDefault { get; }
        private protected FluentState() => IsDefault = true;
        internal static TState Default<TState>() where TState : FluentState, new() => new TState();
    }
}
