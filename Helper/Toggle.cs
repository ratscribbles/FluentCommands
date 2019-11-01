using System;
using System.Collections.Generic;
using System.Text;

namespace FluentCommands.Helper
{
    internal struct Toggle
    {
        private bool _value;

        internal Toggle(bool b) => _value = b;

        public static implicit operator bool(Toggle t) => t._value;

        internal void Flip() => _value = !_value;
    }
}
