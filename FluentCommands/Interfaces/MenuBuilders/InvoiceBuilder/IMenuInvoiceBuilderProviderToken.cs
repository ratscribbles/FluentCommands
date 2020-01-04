using System;
using System.Collections.Generic;
using System.Text;

namespace FluentCommands.Interfaces.MenuBuilders.InvoiceBuilder
{
    public interface IMenuInvoiceBuilderProviderToken : IFluentInterface
    {
        /// <summary>
        /// Required. Provides the Start Parameter for this <see cref="MenuItem"/>.
        /// </summary>
        /// <param name="startParameter">Unique deep-linking parameter that can be used to generate this invoice when used as a start parameter.</param>
        /// <returns>Returns this <see cref="MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuInvoiceBuilderStartParameter StartParameter(string startParameter);
    }
}
