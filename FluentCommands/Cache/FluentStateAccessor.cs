using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCommands.Cache
{
    internal class FluentStateAccessor
    {
        private readonly System.Timers.Timer _timer;
        internal FluentKey Key { get; }
        internal int Duration { get; }
        internal FluentStateAccessor(FluentState state, int duration = 1000)
        {
            _timer = new System.Timers.Timer(duration);
            Key = state.Key;
        }

        //: Cancellation token implementation is needed. This isn't going to work.

        internal bool IsAccessed() => _timer.Enabled;
        internal async Task AccessState() => _timer.Start();
        internal async Task ExitState() => _timer.Stop();
    }
}
