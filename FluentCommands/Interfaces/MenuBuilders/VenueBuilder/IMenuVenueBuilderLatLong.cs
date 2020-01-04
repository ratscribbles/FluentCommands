using System;
using System.Collections.Generic;
using System.Text;

namespace FluentCommands.Interfaces.MenuBuilders.VenueBuilder
{
    public interface IMenuVenueBuilderLatLong : IFluentInterface
    {
        /// <summary>
        /// Required. Name of the venue.
        /// </summary>
        /// <param name="title"></param>
        /// <returns>Returns this <see cref="Menus.Menu"/> to continue fluently building its parameters.</returns>
        IMenuVenueBuilderTitle Title(string title);
    }
}
