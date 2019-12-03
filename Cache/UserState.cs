using System;
using System.Collections.Generic;
using System.Text;

namespace FluentCommands.Cache
{
    public sealed class UserState
    {
        internal StepState StepState { get; private set; } = FluentState.Default<StepState>();
    }
}
