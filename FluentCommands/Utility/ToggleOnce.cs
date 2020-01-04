using System;
using System.Collections.Generic;
using System.Text;

namespace FluentCommands.Utility
{
    internal struct ToggleOnce
    {
        private bool _toggle;
        internal bool HasBeenToggled { get; private set; }
        internal bool Value
        {
            get
            {
                return _toggle;
            }
            set
            {
                if (!HasBeenToggled)
                {   
                    _toggle = value;
                    HasBeenToggled = true;
                }
            }
        }

        internal ToggleOnce(bool b)
        {
            _toggle = b;
            HasBeenToggled = false;
        }

        public static implicit operator bool(ToggleOnce t) => t.Value;
    }
}
