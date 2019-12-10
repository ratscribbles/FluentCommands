using System;
using System.Collections.Generic;
using System.Text;

namespace FluentCommands.Cache
{
    public delegate void MyDelegate(int x);
    public sealed class UserState : FluentState
    {
        public int UserId { get; }
        public StepState StepState { get; private set; } = Default<StepState>();

        public UserState(int id) => UserId = id;
    }
}
