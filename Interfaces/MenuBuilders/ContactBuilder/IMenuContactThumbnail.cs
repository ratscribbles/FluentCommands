using System;
using System.Collections.Generic;
using System.Text;

namespace FluentCommands.Interfaces.MenuBuilders.ContactBuilder
{
    public interface IMenuContactThumbnail : IFluentInterface, IMenuItem
    {
        /// <summary>
        /// Optional. Additional data about the contact in the form of a vCard, 0-2048 bytes.
        /// </summary>
        /// <param name="vCard"></param>
        /// <returns>Returns this <see cref="Menus.MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuItem VCard(string vCard);
    }
}
