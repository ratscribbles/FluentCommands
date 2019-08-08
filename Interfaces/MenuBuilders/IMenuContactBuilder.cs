using System;
using System.Collections.Generic;
using System.Text;
using FluentCommands.Menu;
using FluentCommands.Interfaces.MenuBuilders.ContactBuilder;

namespace FluentCommands.Interfaces.MenuBuilders
{
    public interface IMenuContactBuilder : IFluentInterface
    {
        /// <summary>
        /// Required. Provides the Phone Number of this contact.
        /// </summary>
        /// <param name="phoneNumber">The Phone Number of this contact.</param>
        /// <returns>Returns this <see cref="MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuContactBuilderPhoneNumber PhoneNumber(string phoneNumber);
    }
}
