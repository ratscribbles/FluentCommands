using System;
using System.Collections.Generic;
using System.Text;

namespace FluentCommands.Interfaces.MenuBuilders.ContactBuilder
{
    public interface IMenuContactBuilderPhoneNumber : IFluentInterface
    {
        /// <summary>
        /// Required. Contact's first name.
        /// </summary>
        /// <param name="firstName"></param>
        /// <returns>Returns this <see cref="Menu.MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuContactOptionalBuilder FirstName(string firstName);
    }
}
