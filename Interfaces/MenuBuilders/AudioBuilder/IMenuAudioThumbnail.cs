using System;
using System.Collections.Generic;
using System.Text;

namespace FluentCommands.Interfaces.MenuBuilders.AudioBuilder
{
    public interface IMenuAudioThumbnail : IFluentInterface, ISendableMenu
    {
        /// <summary>
        /// Optional. Track name.
        /// </summary>
        /// <param name="title"></param>
        /// <returns>Returns this <see cref="Menus.Menu"/> to continue fluently building its parameters.</returns>
        Menus.Menu Title(string title);
    }
}
