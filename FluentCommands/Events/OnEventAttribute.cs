using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;

namespace FluentCommands.Events
{
    //: think about the name, but this should be a command that occurs ON AN EVENT. create enums for this; check and see which events can activate with this
    class OnEventAttribute : Attribute
    {
        internal string EventType { get; } //: Implement as Enum later for v1.1
    }
}
