using System;
using System.Collections.Generic;
using System.Text;

namespace FluentCommands.Interfaces.MenuBuilders.AudioBuilder
{
    public interface IMenuAudioThumbnail : IFluentInterface, IMenuItem
    {
        /// <summary>
        /// Optional. Track name.
        /// </summary>
        /// <param name="title"></param>
        /// <returns>Returns this <see cref="Menus.MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuItem Title(string title);
    }
}
