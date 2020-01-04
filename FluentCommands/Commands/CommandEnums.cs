using System;
using System.Collections.Generic;
using System.Text;

namespace FluentCommands.Commands
{
    /// <summary>Determines the "Type" of a <see cref="CommandBase{TContext, TArgs}"/>, which is a quick way to identify the execution of a Command's Context, Args, and possible Return type.</summary>
    internal enum CommandType { Default, Event, Step }
}
