using System;
using System.Collections.Generic;
using System.Text;

namespace FluentCommands.Interfaces.MenuBuilders.InvoiceBuilder
{
    public interface IMenuInvoiceBuilderDescription : IFluentInterface
    {
        /// <summary>
        /// Required. Provides the Payload for this <see cref="MenuItem"/>.
        /// </summary>
        /// <param name="payload">This will not be displayed to the user, use for your internal processes.</param>
        /// <returns>Returns this <see cref="MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuInvoiceBuilderPayload Payload(string payload);
    }
}
