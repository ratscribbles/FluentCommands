using System;
using System.Collections.Generic;
using System.Text;

namespace FluentCommands.Exceptions
{
    /// <summary>
    /// This interface marks an exception as suitable for automatic logging for FluentCommands' CommandLogger.
    /// </summary>
    internal interface IFluentCommandsException
    {
        string? Description { get; }
        Exception? Inner { get; }
    }
}
