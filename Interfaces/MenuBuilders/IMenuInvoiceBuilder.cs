using System;
using System.Collections.Generic;
using System.Text;
using FluentCommands.Menu;
using FluentCommands.Interfaces.MenuBuilders.InvoiceBuilder;

namespace FluentCommands.Interfaces.MenuBuilders
{
    public interface IMenuInvoiceBuilder : IFluentInterface
    {
        /// <summary>
        /// Required. Provides the title (product name) for this <see cref="MenuItem"/>.
        /// <para>1-32 characters.</para>
        /// </summary>
        /// <param name="title">Product name, 1-32 characters.</param>
        /// <returns>Returns this <see cref="MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuInvoiceBuilderTitle Title(string title);
    }
}
