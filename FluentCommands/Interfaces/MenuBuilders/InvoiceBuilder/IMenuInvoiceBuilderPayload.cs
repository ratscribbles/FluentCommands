using System;
using System.Collections.Generic;
using System.Text;

namespace FluentCommands.Interfaces.MenuBuilders.InvoiceBuilder
{
    public interface IMenuInvoiceBuilderPayload : IFluentInterface
    {
        /// <summary>
        /// Required. Provides the ProviderToken for this <see cref="MenuItem"/>.
        /// </summary>
        /// <param name="providerToken">Payments provider token, obtained via Botfather.</param>
        /// <returns>Returns this <see cref="MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuInvoiceBuilderProviderToken ProviderToken(string providerToken);
    }
}
