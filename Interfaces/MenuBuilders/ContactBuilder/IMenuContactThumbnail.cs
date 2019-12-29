using System;
using System.Collections.Generic;
using System.Text;

namespace FluentCommands.Interfaces.MenuBuilders.ContactBuilder
{
    public interface IMenuContactThumbnail : IFluentInterface, ISendableMenu
    {
        /// <summary>
        /// Optional. Additional data about the contact in the form of a vCard, 0-2048 bytes.
        /// </summary>
        /// <param name="vCard"></param>
        /// <returns>Returns this <see cref="Menus.Menu"/> to continue fluently building its parameters.</returns>
        Menus.Menu VCard(string vCard);
    }
}
