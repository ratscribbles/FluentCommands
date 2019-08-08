using System;
using System.Collections.Generic;
using System.Text;

namespace FluentCommands.Interfaces.MenuBuilders.VenueBuilder
{
    public interface IMenuVenueBuilderTitle : IFluentInterface
    {
        IMenuVenueOptionalBuilder Address(string address);
    }
}
