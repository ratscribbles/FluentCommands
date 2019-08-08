using System;
using System.Collections.Generic;
using System.Text;

namespace FluentCommands.Interfaces.MenuBuilders.VenueBuilder
{
    public interface IMenuVenueBuilderLongitude : IFluentInterface
    {
        IMenuVenueBuilderTitle Title(string title);
    }
}
