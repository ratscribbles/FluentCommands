using FluentCommands.Interfaces.KeyboardBuilders;
using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types;

namespace FluentCommands.Interfaces.MenuBuilders.InvoiceBuilder
{
    public interface IMenuInvoicePhotoHeight : IReplyMarkupableForceInline<IMenuInvoiceReplyMarkup>, IFluentInterface, ISendableMenu
    {
        /// <summary>
        /// Optional. Photo size.
        /// </summary>
        /// <param name="photoSize"></param>
        /// <returns>Returns this <see cref="Menus.Menu"/> to continue fluently building its parameters.</returns>
        IMenuInvoicePhotoSize PhotoSize(int photoSize);
        /// <summary>
        /// Optional. Photo width.
        /// </summary>
        /// <param name="photoWidth"></param>
        /// <returns>Returns this <see cref="Menus.Menu"/> to continue fluently building its parameters.</returns>
        IMenuInvoicePhotoWidth PhotoWidth(int photoWidth);
        /// <summary>
        /// Optional. URL of the product photo for the invoice. 
        /// <para>Can be a photo of the goods or a marketing image for a service. People like it better when they see what they are paying for.</para>
        /// </summary>
        /// <param name="photoUrl"></param>
        /// <returns>Returns this <see cref="Menus.Menu"/> to continue fluently building its parameters.</returns>
        IMenuInvoicePhotoUrl PhotoUrl(string photoUrl);
        /// <summary>
        /// Optional. JSON-encoded data about the invoice, which will be shared with the payment provider.
        /// <para>A detailed description of required fields should be provided by the payment provider.</para>
        /// </summary>
        /// <param name="providerData"></param>
        /// <returns>Returns this <see cref="Menus.Menu"/> to continue fluently building its parameters.</returns>
        IMenuInvoiceProviderData ProviderData(string providerData);
        /// <summary>
        /// Optional. If the message is a reply, Message object or ID of the original message.
        /// </summary>
        /// <param name="message"></param>
        /// <returns>Returns this <see cref="Menus.Menu"/> to continue fluently building its parameters.</returns>
        Menus.Menu ReplyToMessage(Message message);
        /// <summary>
        /// Optional. If the message is a reply, Message object or ID of the original message.
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns>Returns this <see cref="Menus.Menu"/> to continue fluently building its parameters.</returns>
        Menus.Menu ReplyToMessage(int messageId);
    }
}
