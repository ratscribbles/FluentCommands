using System;
using System.Collections.Generic;
using System.Text;

namespace FluentCommands.Interfaces.MenuBuilders.InvoiceBuilder
{
    public interface IMenuInvoiceBuilderTitle : IFluentInterface
    {
        /// <summary>
        /// Required. Provides the Description for this <see cref="MenuItem"/>.
        /// </summary>
        /// <param name="description">Product description, 1-255 characters.</param>
        /// <returns>Returns this <see cref="MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuInvoiceBuilderDescription Description(string description);
    }
}
