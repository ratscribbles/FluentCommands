using System;
using System.Collections.Generic;
using System.Text;

namespace FluentCommands.Interfaces.MenuBuilders.LocationBuilder
{
    public interface IMenuLocationBuilderLatitude : IFluentInterface
    {
        /// <summary>
        /// Required. Longitude of the location.
        /// </summary>
        /// <param name="longitude"></param>
        /// <returns>Returns this <see cref="Menus.MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuLocationOptionalBuilder Longitude(float longitude);
    }
}
