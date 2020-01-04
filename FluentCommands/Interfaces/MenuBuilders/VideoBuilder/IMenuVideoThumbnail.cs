using System;
using System.Collections.Generic;
using System.Text;

namespace FluentCommands.Interfaces.MenuBuilders.VideoBuilder
{
    public interface IMenuVideoThumbnail : IFluentInterface, ISendableMenu
    {
        /// <summary>
        /// Optional. Video width.
        /// </summary>
        /// <param name="width"></param>
        /// <returns>Returns this <see cref="Menus.Menu"/> to continue fluently building its parameters.</returns>
        Menus.Menu Width(int width);
    }
}
