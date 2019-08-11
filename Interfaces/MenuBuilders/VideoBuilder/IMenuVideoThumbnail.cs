using System;
using System.Collections.Generic;
using System.Text;

namespace FluentCommands.Interfaces.MenuBuilders.VideoBuilder
{
    public interface IMenuVideoThumbnail : IFluentInterface, IMenuItem
    {
        /// <summary>
        /// Optional. Video width.
        /// </summary>
        /// <param name="width"></param>
        /// <returns>Returns this <see cref="Menus.MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuItem Width(int width);
    }
}
