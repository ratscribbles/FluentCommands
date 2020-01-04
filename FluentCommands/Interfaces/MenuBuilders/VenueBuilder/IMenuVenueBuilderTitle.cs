using System;
using System.Collections.Generic;
using System.Text;

namespace FluentCommands.Interfaces.MenuBuilders.VenueBuilder
{
    public interface IMenuVenueBuilderTitle : IFluentInterface
    {
        /// <summary>
        /// Required. Address of the venue.
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        IMenuVenueOptionalBuilder Address(string address);
    }
}
