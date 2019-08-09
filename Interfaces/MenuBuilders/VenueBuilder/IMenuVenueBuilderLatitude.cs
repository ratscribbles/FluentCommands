using System;
using System.Collections.Generic;
using System.Text;

namespace FluentCommands.Interfaces.MenuBuilders.VenueBuilder
{
    public interface IMenuVenueBuilderLatitude : IFluentInterface
    {
        /// <summary>
        /// Required. Longitude of the venue.
        /// </summary>
        /// <param name="longitude"></param>
        /// <returns>Returns this <see cref="Menu.MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuVenueBuilderLongitude Longitude(float longitude);
    }
}
