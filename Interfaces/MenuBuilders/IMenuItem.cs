using System;
using System.Collections.Generic;
using System.Text;
using FluentCommands.Menus;

namespace FluentCommands.Interfaces.MenuBuilders
{
    /// <summary>
    /// Determines an interface to be one that can be casted to <see cref="MenuItem"/>.
    /// </summary>
    public interface IMenuItem
    {
        /// <summary>
        /// Marks the <see cref="MenuItem"/> as complete, and suitable for sending.
        /// <para>Allows you specify where to send this <see cref="MenuItem"/>.</para>
        /// </summary>
        /// <returns>Returns this completed <see cref="MenuItem"/> as a <see cref="Menu"/> object.</returns>
        Menu SendTo(int idToSendTo);

        /// <summary>
        /// Marks the <see cref="MenuItem"/> as complete, and suitable for sending.
        /// <para>Allows you specify where to send this <see cref="MenuItem"/>.</para>
        /// </summary>
        /// <returns>Returns this completed <see cref="MenuItem"/> as a <see cref="Menu"/> object.</returns>
        Menu SendTo(long idToSendTo);
    }
}
