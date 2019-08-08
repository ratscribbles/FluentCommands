using System;
using System.Collections.Generic;
using System.Text;

namespace FluentCommands.Interfaces.MenuBuilders.AnimationBuilder
{
    public interface IMenuAnimationThumbnail : IFluentInterface, IMenuItem
    {
        /// <summary>
        /// Optional. Animation width.
        /// </summary>
        /// <param name="width"></param>
        /// <returns>Returns this <see cref="Menu.MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuItem Width(int width);
    }
}
