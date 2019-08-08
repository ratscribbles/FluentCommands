using System;
using System.Collections.Generic;
using System.Text;
using FluentCommands.Menu;
using FluentCommands.Interfaces.MenuBuilders.TextBuilder;

namespace FluentCommands.Interfaces.MenuBuilders
{
    public interface IMenuTextBuilder : IFluentInterface
    {
        /// <summary>
        /// Required. Provides the text of the message to be sent.
        /// </summary>
        /// <param name="text"></param>
        /// <returns>Returns this <see cref="MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuTextOptionalBuilder TextSource(string text);
    }
}
