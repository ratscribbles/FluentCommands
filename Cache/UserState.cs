using System;
using System.Collections.Generic;
using System.Text;

namespace FluentCommands.Cache
{
    public sealed class UserState : FluentState
    {
        public int UserId { get; }
        public StepState StepState { get; private set; } = FluentState.Default<StepState>();

        public UserState(int id) => UserId = id;
    }
}
