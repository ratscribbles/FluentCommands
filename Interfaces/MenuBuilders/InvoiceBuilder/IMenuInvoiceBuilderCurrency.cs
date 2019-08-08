using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types.Payments;

namespace FluentCommands.Interfaces.MenuBuilders.InvoiceBuilder
{
    public interface IMenuInvoiceBuilderCurrency : IFluentInterface
    {
        /// <summary>
        /// Required. Provides the Prices for this <see cref="MenuItem"/>.
        /// </summary>
        /// <param name="prices">Price breakdown, a list of components (e.g. product price, tax, discount, delivery cost, delivery tax, bonus, etc.).</param>
        /// <returns>Returns this <see cref="MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuInvoiceOptionalBuilder Prices(params LabeledPrice[] prices);
        /// <summary>
        /// Required. Provides the Prices for this <see cref="MenuItem"/>.
        /// </summary>
        /// <param name="prices">Price breakdown, a list of components (e.g. product price, tax, discount, delivery cost, delivery tax, bonus, etc.).</param>
        /// <returns>Returns this <see cref="MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuInvoiceOptionalBuilder Prices(IEnumerable<LabeledPrice> prices);
    }
}
