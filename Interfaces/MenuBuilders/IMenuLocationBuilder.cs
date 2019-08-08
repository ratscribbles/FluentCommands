using System;
using System.Collections.Generic;
using System.Text;
using FluentCommands.Menu;
using FluentCommands.Interfaces.MenuBuilders.LocationBuilder;

namespace FluentCommands.Interfaces.MenuBuilders
{
    public interface IMenuLocationBuilder : IFluentInterface
    {
        /// <summary>
        /// Required. The latitude for this Location.
        /// </summary>
        /// <param name="latitude"></param>
        /// <returns>Returns this <see cref="MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuLocationBuilderLatitude Latitude(float latitude);
    }
}
