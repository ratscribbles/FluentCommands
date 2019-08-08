using System;
using System.Collections.Generic;
using System.Text;

namespace FluentCommands.Interfaces.MenuBuilders.VenueBuilder
{
    public interface IMenuVenueBuilderLatitude : IFluentInterface
    {
        IMenuVenueBuilderLongitude Longitude(float longitude);
    }
}
