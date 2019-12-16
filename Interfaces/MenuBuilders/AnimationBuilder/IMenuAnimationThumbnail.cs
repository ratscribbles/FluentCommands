using System;
using System.Collections.Generic;
using System.Text;

namespace FluentCommands.Interfaces.MenuBuilders.AnimationBuilder
{
    public interface IMenuAnimationThumbnail : IFluentInterface, IMenu
    {
        /// <summary>
        /// Optional. Animation width.
        /// </summary>
        /// <param name="width"></param>
        /// <returns>Returns this <see cref="Menus.Menu"/> to continue fluently building its parameters.</returns>
        IMenu Width(int width);
    }
}
