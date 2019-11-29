using System;
using System.Collections.Generic;
using System.Text;

namespace FluentCommands.Helper
{
    // Special thanks to the creator of Finite Commands for help with this
    internal sealed class CommandNameComparer : IEqualityComparer<ReadOnlyMemory<char>> 
    {
        public static CommandNameComparer Default { get; } = new CommandNameComparer();

        private CommandNameComparer() { }

        public bool Equals(ReadOnlyMemory<char> x, ReadOnlyMemory<char> y)
        {
            return x.Span.SequenceEqual(y.Span);
        }

        public int GetHashCode(ReadOnlyMemory<char> obj)
        {
            var code = new HashCode();
            var span = obj.Span;

            for (int x = 0; x < obj.Length; x++)
            {
                code.Add(span[x]);
            }

            return code.ToHashCode();
        }
    }
}
