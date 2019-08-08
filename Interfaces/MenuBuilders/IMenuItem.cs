using System;
using System.Collections.Generic;
using System.Text;
using FluentCommands.Menu;

namespace FluentCommands.Interfaces.MenuBuilders
{
    /// <summary>
    /// Determines an interface to be one that can be casted to <see cref="MenuItem"/>.
    /// </summary>
    public interface IMenuItem
    {
        /// <summary>
        /// Marks the <see cref="MenuItem"/> as complete, and suitable for sending.
        /// </summary>
        /// <returns>Returns this completed <see cref="MenuItem"/>.</returns>
        MenuItem Done();
    }
}
