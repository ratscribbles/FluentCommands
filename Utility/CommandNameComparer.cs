using System;
using System.Collections.Generic;
using System.Text;

namespace FluentCommands.Utility
{
    // Special thanks to the creator of Finite Commands for helping me think about how to implement this
    internal sealed class CommandNameComparer : IEqualityComparer<ReadOnlyMemory<char>> 
    {
        public static CommandNameComparer Default { get; } = new CommandNameComparer();

        private CommandNameComparer() { }

        public bool Equals(ReadOnlyMemory<char> x, ReadOnlyMemory<char> y) => x.Span.SequenceEqual(y.Span);

        public int GetHashCode(ReadOnlyMemory<char> obj)
        {
            var span = obj.Span;

            unchecked
            {
                // Choose large primes to avoid hashing collisions
                const int HashingBase = (int)2166136261;
                const int HashingMultiplier = 16777619;

                int hash = HashingBase;
                for (int x = 0; x < obj.Length; x++)
                {
                    hash = (hash * HashingMultiplier) ^ span[x].GetHashCode();
                }

                return hash;
            }
        }
    }
}
