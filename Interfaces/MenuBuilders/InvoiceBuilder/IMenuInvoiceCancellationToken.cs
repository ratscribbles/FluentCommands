using FluentCommands.Interfaces.KeyboardBuilders;
using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types;

namespace FluentCommands.Interfaces.MenuBuilders.InvoiceBuilder
{
    public interface IMenuInvoiceCancellationToken : IReplyMarkupableForceInline<IMenuInvoiceReplyMarkup>, IFluentInterface, IMenuItem
    {
        /// <summary>
        /// Optional. Sends the message silently. Users will receive a notification with no sound.
        /// </summary>
        /// <param name="disableNotification"></param>
        /// <returns>Returns this <see cref="Menus.MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuInvoiceDisableNotification DisableNotification(bool disableNotification);
        /// <summary>
        /// Optional. Pass <c>true</c> if the final price depends on the shipping method.
        /// </summary>
        /// <param name="isFlexible"></param>
        /// <returns>Returns this <see cref="Menus.MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuInvoiceIsFlexible IsFlexible(bool isFlexible);
        /// <summary>
        /// Optional. Pass <c>true</c> if you require the user's email address to complete the order.
        /// </summary>
        /// <param name="needsEmail"></param>
        /// <returns>Returns this <see cref="Menus.MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuInvoiceNeedsEmail NeedsEmail(bool needsEmail);
        /// <summary>
        /// Optional. Pass <c>true</c> if you require the user's full name to complete the order.
        /// </summary>
        /// <param name="needsName"></param>
        /// <returns>Returns this <see cref="Menus.MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuInvoiceNeedsName NeedsName(bool needsName);
        /// <summary>
        /// Optional. Pass <c>true</c> if you require the user's phone number to complete the order.
        /// </summary>
        /// <param name="needsPhoneNumber"></param>
        /// <returns>Returns this <see cref="Menus.MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuInvoiceNeedsPhoneNumber NeedsPhoneNumber(bool needsPhoneNumber);
        /// <summary>
        /// Optional. Pass <c>true</c> if you require the user's shipping address to complete the order.
        /// </summary>
        /// <param name="needsShippingAddress"></param>
        /// <returns>Returns this <see cref="Menus.MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuInvoiceNeedsShippingAddress NeedsShippingAddress(bool needsShippingAddress);
        /// <summary>
        /// Optional. Photo height.
        /// </summary>
        /// <param name="photoHeight"></param>
        /// <returns>Returns this <see cref="Menus.MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuInvoicePhotoHeight PhotoHeight(int photoHeight);
        /// <summary>
        /// Optional. Photo size.
        /// </summary>
        /// <param name="photoSize"></param>
        /// <returns>Returns this <see cref="Menus.MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuInvoicePhotoSize PhotoSize(int photoSize);
        /// <summary>
        /// Optional. Photo width.
        /// </summary>
        /// <param name="photoWidth"></param>
        /// <returns>Returns this <see cref="Menus.MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuInvoicePhotoWidth PhotoWidth(int photoWidth);
        /// <summary>
        /// Optional. URL of the product photo for the invoice. 
        /// <para>Can be a photo of the goods or a marketing image for a service. People like it better when they see what they are paying for.</para>
        /// </summary>
        /// <param name="photoUrl"></param>
        /// <returns>Returns this <see cref="Menus.MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuInvoicePhotoUrl PhotoUrl(string photoUrl);
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
