using System;
using System.Collections.Generic;
using System.Text;

namespace FluentCommands.Interfaces.MenuBuilders.InvoiceBuilder
{
    public interface IMenuInvoiceBuilderStartParameter : IFluentInterface
    {
        /// <summary>
        /// Required. Provides the Currency for this <see cref="MenuItem"/>.
        /// </summary>
        /// <param name="currency">Three-letter ISO 4217 currency code.</param>
        /// <returns>Returns this <see cref="MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuInvoiceBuilderCurrency Currency(string currency);
    }
}
