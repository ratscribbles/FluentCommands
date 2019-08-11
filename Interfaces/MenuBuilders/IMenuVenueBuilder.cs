using System;
using System.Collections.Generic;
using System.Text;
using FluentCommands.Menus;
using FluentCommands.Interfaces.MenuBuilders.VenueBuilder;

namespace FluentCommands.Interfaces.MenuBuilders
{
    public interface IMenuVenueBuilder : IFluentInterface
    {
        /// <summary>
        /// Required. Provides the latitude for this Venue.
        /// </summary>
        /// <param name="latitude"></param>
        /// <returns>Returns this <see cref="MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuVenueBuilderLatitude Latitude(float latitude);
    }
}
