using System;
using System.Collections.Generic;
using System.Text;

namespace FluentCommands
{
    public class ChainResult<T>
    {
        public T Value { get; set; }

        internal ChainResult(T value) => Value = value;

        public static implicit operator ChainResult<T>(T value) => new ChainResult<T>(value);
        public override string ToString() => Value?.ToString() ?? "";
    }
}
