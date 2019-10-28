using FluentCommands.Interfaces.KeyboardBuilders;
using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types;

namespace FluentCommands.Interfaces.MenuBuilders.InvoiceBuilder
{
    public interface IMenuInvoicePhotoWidth : IReplyMarkupableForceInline<IMenuInvoiceReplyMarkup>, IFluentInterface, IMenuItem
    {
        /// <summary>
        /// Optional. JSON-encoded data about the invoice, which will be shared with the payment provider.
        /// <para>A detailed description of required fields should be provided by the payment provider.</para>
        /// </summary>
        /// <param name="providerData"></param>
        /// <returns>Returns this <see cref="Menus.MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuInvoiceProviderData ProviderData(string providerData);
        /// <summary>
        /// Optional. If the message is a reply, Message object or ID of the original message.
        /// </summary>
        /// <param name="message"></param>
        /// <returns>Returns this <see cref="Menus.MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuItem ReplyToMessage(Message message);
        /// <summary>
        /// Optional. If the message is a reply, Message object or ID of the original message.
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns>Returns this <see cref="Menus.MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuItem ReplyToMessage(int messageId);
    }
}
