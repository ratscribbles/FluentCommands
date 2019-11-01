using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Payments;
using FluentCommands.Interfaces;
using FluentCommands.Interfaces.MenuBuilders;
using FluentCommands.Interfaces.MenuBuilders.InvoiceBuilder;
using FluentCommands.Interfaces.KeyboardBuilders;
using Telegram.Bot.Types.ReplyMarkups;
using FluentCommands.Builders;

namespace FluentCommands.Menus
{
    public partial class MenuItem : IMenuInvoiceBuilder, IMenuInvoiceOptionalBuilder,
        IMenuInvoiceBuilderCurrency, IMenuInvoiceBuilderDescription, IMenuInvoiceBuilderPayload, IMenuInvoiceBuilderProviderToken, IMenuInvoiceBuilderStartParameter, IMenuInvoiceBuilderTitle,
        IMenuInvoiceCancellationToken, IMenuInvoiceDisableNotification, IMenuInvoiceIsFlexible, IMenuInvoiceNeedsEmail, IMenuInvoiceNeedsName, IMenuInvoiceNeedsPhoneNumber, IMenuInvoiceNeedsShippingAddress, IMenuInvoicePhotoHeight, IMenuInvoicePhotoSize, IMenuInvoicePhotoUrl, IMenuInvoicePhotoWidth, IMenuInvoiceProviderData,
        IMenuInvoiceReplyMarkup, IKeyboardBuilderForceInline<IMenuInvoiceReplyMarkup>
    {
        #region Required
        IMenuInvoiceBuilderTitle IMenuInvoiceBuilder.Title(string title) { Title = title; return this; }
        IMenuInvoiceBuilderPayload IMenuInvoiceBuilderDescription.Payload(string payload) { Payload = payload; return this; }
        IMenuInvoiceBuilderProviderToken IMenuInvoiceBuilderPayload.ProviderToken(string providerToken) { ProviderToken = providerToken; return this; }
        IMenuInvoiceBuilderStartParameter IMenuInvoiceBuilderProviderToken.StartParameter(string startParameter) { StartParameter = startParameter; return this; }
        IMenuInvoiceBuilderCurrency IMenuInvoiceBuilderStartParameter.Currency(string currency) { Currency = currency; return this; }
        IMenuInvoiceBuilderDescription IMenuInvoiceBuilderTitle.Description(string description) { Description = description; return this; }
        IMenuInvoiceOptionalBuilder IMenuInvoiceBuilderCurrency.Prices(params LabeledPrice[] prices) { Prices = prices; return this; }
        IMenuInvoiceOptionalBuilder IMenuInvoiceBuilderCurrency.Prices(IEnumerable<LabeledPrice> prices) { Prices = prices; return this; }
        #endregion

        #region Optional
        IMenuInvoiceCancellationToken IMenuInvoiceOptionalBuilder.CancellationToken(CancellationToken token) { Token = token; return this; }
        IMenuInvoiceDisableNotification IMenuInvoiceOptionalBuilder.DisableNotification(bool disableNotification) { DisableNotification = disableNotification; return this; }
        IMenuInvoiceIsFlexible IMenuInvoiceOptionalBuilder.IsFlexible(bool isFlexible) { IsFlexibile = isFlexible; return this; }
        IMenuInvoiceNeedsEmail IMenuInvoiceOptionalBuilder.NeedsEmail(bool needsEmail) { NeedsEmail = needsEmail; return this; }
        IMenuInvoiceNeedsName IMenuInvoiceOptionalBuilder.NeedsName(bool needsName) { NeedsName = needsName; return this; }
        IMenuInvoiceNeedsPhoneNumber IMenuInvoiceOptionalBuilder.NeedsPhoneNumber(bool needsPhoneNumber) { NeedsPhoneNumber = needsPhoneNumber; return this; }
        IMenuInvoiceNeedsShippingAddress IMenuInvoiceOptionalBuilder.NeedsShippingAddress(bool needsShippingAddress) { NeedsShippingAddress = needsShippingAddress; return this; }
        IMenuInvoicePhotoHeight IMenuInvoiceOptionalBuilder.PhotoHeight(int photoHeight) { PhotoHeight = photoHeight; return this; }
        IMenuInvoicePhotoSize IMenuInvoiceOptionalBuilder.PhotoSize(int photoSize) { PhotoSize = photoSize; return this; }
        IMenuInvoicePhotoWidth IMenuInvoiceOptionalBuilder.PhotoWidth(int photoWidth) { PhotoWidth = photoWidth; return this; }
        IMenuInvoicePhotoUrl IMenuInvoiceOptionalBuilder.PhotoUrl(string photoUrl) { PhotoUrl = photoUrl; return this; }
        IMenuInvoiceProviderData IMenuInvoiceOptionalBuilder.ProviderData(string providerData) { ProviderData = providerData; return this; }
        IMenuItem IMenuInvoiceOptionalBuilder.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuItem IMenuInvoiceOptionalBuilder.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        #endregion

        #region Additional Implementation
        IMenuInvoiceDisableNotification IMenuInvoiceCancellationToken.DisableNotification(bool disableNotification) { DisableNotification = disableNotification; return this; }
        IMenuInvoiceIsFlexible IMenuInvoiceCancellationToken.IsFlexible(bool isFlexible) { IsFlexibile = isFlexible; return this; }
        IMenuInvoiceNeedsEmail IMenuInvoiceCancellationToken.NeedsEmail(bool needsEmail) { NeedsEmail = needsEmail; return this; }
        IMenuInvoiceNeedsName IMenuInvoiceCancellationToken.NeedsName(bool needsName) { NeedsName = needsName; return this; }
        IMenuInvoiceNeedsPhoneNumber IMenuInvoiceCancellationToken.NeedsPhoneNumber(bool needsPhoneNumber) { NeedsPhoneNumber = needsPhoneNumber; return this; }
        IMenuInvoiceNeedsShippingAddress IMenuInvoiceCancellationToken.NeedsShippingAddress(bool needsShippingAddress) { NeedsShippingAddress = needsShippingAddress; return this; }
        IMenuInvoicePhotoHeight IMenuInvoiceCancellationToken.PhotoHeight(int photoHeight) { PhotoHeight = photoHeight; return this; }
        IMenuInvoicePhotoSize IMenuInvoiceCancellationToken.PhotoSize(int photoSize) { PhotoSize = photoSize; return this; }
        IMenuInvoicePhotoWidth IMenuInvoiceCancellationToken.PhotoWidth(int photoWidth) { PhotoWidth = photoWidth; return this; }
        IMenuInvoicePhotoUrl IMenuInvoiceCancellationToken.PhotoUrl(string photoUrl) { PhotoUrl = photoUrl; return this; }
        IMenuInvoiceProviderData IMenuInvoiceCancellationToken.ProviderData(string providerData) { ProviderData = providerData; return this; }
        IMenuItem IMenuInvoiceCancellationToken.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuItem IMenuInvoiceCancellationToken.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        ////
        IMenuInvoiceIsFlexible IMenuInvoiceDisableNotification.IsFlexible(bool isFlexible) { IsFlexibile = isFlexible; return this; }
        IMenuInvoiceNeedsEmail IMenuInvoiceDisableNotification.NeedsEmail(bool needsEmail) { NeedsEmail = needsEmail; return this; }
        IMenuInvoiceNeedsName IMenuInvoiceDisableNotification.NeedsName(bool needsName) { NeedsName = needsName; return this; }
        IMenuInvoiceNeedsPhoneNumber IMenuInvoiceDisableNotification.NeedsPhoneNumber(bool needsPhoneNumber) { NeedsPhoneNumber = needsPhoneNumber; return this; }
        IMenuInvoiceNeedsShippingAddress IMenuInvoiceDisableNotification.NeedsShippingAddress(bool needsShippingAddress) { NeedsShippingAddress = needsShippingAddress; return this; }
        IMenuInvoicePhotoHeight IMenuInvoiceDisableNotification.PhotoHeight(int photoHeight) { PhotoHeight = photoHeight; return this; }
        IMenuInvoicePhotoSize IMenuInvoiceDisableNotification.PhotoSize(int photoSize) { PhotoSize = photoSize; return this; }
        IMenuInvoicePhotoWidth IMenuInvoiceDisableNotification.PhotoWidth(int photoWidth) { PhotoWidth = photoWidth; return this; }
        IMenuInvoicePhotoUrl IMenuInvoiceDisableNotification.PhotoUrl(string photoUrl) { PhotoUrl = photoUrl; return this; }
        IMenuInvoiceProviderData IMenuInvoiceDisableNotification.ProviderData(string providerData) { ProviderData = providerData; return this; }
        IMenuItem IMenuInvoiceDisableNotification.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuItem IMenuInvoiceDisableNotification.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        ////
        IMenuInvoiceNeedsEmail IMenuInvoiceIsFlexible.NeedsEmail(bool needsEmail) { NeedsEmail = needsEmail; return this; }
        IMenuInvoiceNeedsName IMenuInvoiceIsFlexible.NeedsName(bool needsName) { NeedsName = needsName; return this; }
        IMenuInvoiceNeedsPhoneNumber IMenuInvoiceIsFlexible.NeedsPhoneNumber(bool needsPhoneNumber) { NeedsPhoneNumber = needsPhoneNumber; return this; }
        IMenuInvoiceNeedsShippingAddress IMenuInvoiceIsFlexible.NeedsShippingAddress(bool needsShippingAddress) { NeedsShippingAddress = needsShippingAddress; return this; }
        IMenuInvoicePhotoHeight IMenuInvoiceIsFlexible.PhotoHeight(int photoHeight) { PhotoHeight = photoHeight; return this; }
        IMenuInvoicePhotoSize IMenuInvoiceIsFlexible.PhotoSize(int photoSize) { PhotoSize = photoSize; return this; }
        IMenuInvoicePhotoWidth IMenuInvoiceIsFlexible.PhotoWidth(int photoWidth) { PhotoWidth = photoWidth; return this; }
        IMenuInvoicePhotoUrl IMenuInvoiceIsFlexible.PhotoUrl(string photoUrl) { PhotoUrl = photoUrl; return this; }
        IMenuInvoiceProviderData IMenuInvoiceIsFlexible.ProviderData(string providerData) { ProviderData = providerData; return this; }
        IMenuItem IMenuInvoiceIsFlexible.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuItem IMenuInvoiceIsFlexible.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        ////
        IMenuInvoiceNeedsName IMenuInvoiceNeedsEmail.NeedsName(bool needsName) { NeedsName = needsName; return this; }
        IMenuInvoiceNeedsPhoneNumber IMenuInvoiceNeedsEmail.NeedsPhoneNumber(bool needsPhoneNumber) { NeedsPhoneNumber = needsPhoneNumber; return this; }
        IMenuInvoiceNeedsShippingAddress IMenuInvoiceNeedsEmail.NeedsShippingAddress(bool needsShippingAddress) { NeedsShippingAddress = needsShippingAddress; return this; }
        IMenuInvoicePhotoHeight IMenuInvoiceNeedsEmail.PhotoHeight(int photoHeight) { PhotoHeight = photoHeight; return this; }
        IMenuInvoicePhotoSize IMenuInvoiceNeedsEmail.PhotoSize(int photoSize) { PhotoSize = photoSize; return this; }
        IMenuInvoicePhotoWidth IMenuInvoiceNeedsEmail.PhotoWidth(int photoWidth) { PhotoWidth = photoWidth; return this; }
        IMenuInvoicePhotoUrl IMenuInvoiceNeedsEmail.PhotoUrl(string photoUrl) { PhotoUrl = photoUrl; return this; }
        IMenuInvoiceProviderData IMenuInvoiceNeedsEmail.ProviderData(string providerData) { ProviderData = providerData; return this; }
        IMenuItem IMenuInvoiceNeedsEmail.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuItem IMenuInvoiceNeedsEmail.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        ////
        IMenuInvoiceNeedsPhoneNumber IMenuInvoiceNeedsName.NeedsPhoneNumber(bool needsPhoneNumber) { NeedsPhoneNumber = needsPhoneNumber; return this; }
        IMenuInvoiceNeedsShippingAddress IMenuInvoiceNeedsName.NeedsShippingAddress(bool needsShippingAddress) { NeedsShippingAddress = needsShippingAddress; return this; }
        IMenuInvoicePhotoHeight IMenuInvoiceNeedsName.PhotoHeight(int photoHeight) { PhotoHeight = photoHeight; return this; }
        IMenuInvoicePhotoSize IMenuInvoiceNeedsName.PhotoSize(int photoSize) { PhotoSize = photoSize; return this; }
        IMenuInvoicePhotoWidth IMenuInvoiceNeedsName.PhotoWidth(int photoWidth) { PhotoWidth = photoWidth; return this; }
        IMenuInvoicePhotoUrl IMenuInvoiceNeedsName.PhotoUrl(string photoUrl) { PhotoUrl = photoUrl; return this; }
        IMenuInvoiceProviderData IMenuInvoiceNeedsName.ProviderData(string providerData) { ProviderData = providerData; return this; }
        IMenuItem IMenuInvoiceNeedsName.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuItem IMenuInvoiceNeedsName.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        ////
        IMenuInvoiceNeedsShippingAddress IMenuInvoiceNeedsPhoneNumber.NeedsShippingAddress(bool needsShippingAddress) { NeedsShippingAddress = needsShippingAddress; return this; }
        IMenuInvoicePhotoHeight IMenuInvoiceNeedsPhoneNumber.PhotoHeight(int photoHeight) { PhotoHeight = photoHeight; return this; }
        IMenuInvoicePhotoSize IMenuInvoiceNeedsPhoneNumber.PhotoSize(int photoSize) { PhotoSize = photoSize; return this; }
        IMenuInvoicePhotoWidth IMenuInvoiceNeedsPhoneNumber.PhotoWidth(int photoWidth) { PhotoWidth = photoWidth; return this; }
        IMenuInvoicePhotoUrl IMenuInvoiceNeedsPhoneNumber.PhotoUrl(string photoUrl) { PhotoUrl = photoUrl; return this; }
        IMenuInvoiceProviderData IMenuInvoiceNeedsPhoneNumber.ProviderData(string providerData) { ProviderData = providerData; return this; }
        IMenuItem IMenuInvoiceNeedsPhoneNumber.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuItem IMenuInvoiceNeedsPhoneNumber.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        ////
        IMenuInvoicePhotoHeight IMenuInvoiceNeedsShippingAddress.PhotoHeight(int photoHeight) { PhotoHeight = photoHeight; return this; }
        IMenuInvoicePhotoSize IMenuInvoiceNeedsShippingAddress.PhotoSize(int photoSize) { PhotoSize = photoSize; return this; }
        IMenuInvoicePhotoWidth IMenuInvoiceNeedsShippingAddress.PhotoWidth(int photoWidth) { PhotoWidth = photoWidth; return this; }
        IMenuInvoicePhotoUrl IMenuInvoiceNeedsShippingAddress.PhotoUrl(string photoUrl) { PhotoUrl = photoUrl; return this; }
        IMenuInvoiceProviderData IMenuInvoiceNeedsShippingAddress.ProviderData(string providerData) { ProviderData = providerData; return this; }
        IMenuItem IMenuInvoiceNeedsShippingAddress.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuItem IMenuInvoiceNeedsShippingAddress.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        ////
        IMenuInvoicePhotoSize IMenuInvoicePhotoHeight.PhotoSize(int photoSize) { PhotoSize = photoSize; return this; }
        IMenuInvoicePhotoWidth IMenuInvoicePhotoHeight.PhotoWidth(int photoWidth) { PhotoWidth = photoWidth; return this; }
        IMenuInvoicePhotoUrl IMenuInvoicePhotoHeight.PhotoUrl(string photoUrl) { PhotoUrl = photoUrl; return this; }
        IMenuInvoiceProviderData IMenuInvoicePhotoHeight.ProviderData(string providerData) { ProviderData = providerData; return this; }
        IMenuItem IMenuInvoicePhotoHeight.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuItem IMenuInvoicePhotoHeight.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        ////
        IMenuInvoicePhotoWidth IMenuInvoicePhotoSize.PhotoWidth(int photoWidth) { PhotoWidth = photoWidth; return this; }
        IMenuInvoicePhotoUrl IMenuInvoicePhotoSize.PhotoUrl(string photoUrl) { PhotoUrl = photoUrl; return this; }
        IMenuInvoiceProviderData IMenuInvoicePhotoSize.ProviderData(string providerData) { ProviderData = providerData; return this; }
        IMenuItem IMenuInvoicePhotoSize.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuItem IMenuInvoicePhotoSize.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        ////
        IMenuInvoicePhotoUrl IMenuInvoicePhotoUrl.PhotoUrl(string photoUrl) { PhotoUrl = photoUrl; return this; }
        IMenuInvoiceProviderData IMenuInvoicePhotoUrl.ProviderData(string providerData) { ProviderData = providerData; return this; }
        IMenuItem IMenuInvoicePhotoUrl.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuItem IMenuInvoicePhotoUrl.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        ////
        IMenuInvoiceProviderData IMenuInvoicePhotoWidth.ProviderData(string providerData) { ProviderData = providerData; return this; }
        IMenuItem IMenuInvoicePhotoWidth.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuItem IMenuInvoicePhotoWidth.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        ////
        IMenuItem IMenuInvoiceProviderData.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuItem IMenuInvoiceProviderData.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        ////
        IMenuItem IMenuInvoiceReplyMarkup.ReplyToMessage(Message message) { ReplyToMessage = message; return this; }
        IMenuItem IMenuInvoiceReplyMarkup.ReplyToMessage(int messageId) { ReplyToMessage = new Message { MessageId = messageId }; return this; }
        #endregion

        #region Keyboard Implementation
        IKeyboardBuilderForceInline<IMenuInvoiceReplyMarkup> IReplyMarkupableForceInline<IMenuInvoiceReplyMarkup>.ReplyMarkup() => this;

        IMenuInvoiceReplyMarkup IReplyMarkupableForceInline<IMenuInvoiceReplyMarkup>.ReplyMarkup(InlineKeyboardMarkup markup) { ReplyMarkup = markup; return this; }

        IMenuInvoiceReplyMarkup IKeyboardBuilderForceInline<IMenuInvoiceReplyMarkup>.Inline(Action<IInlineKeyboardBuilder> buildAction)
        {
            KeyboardBuilder keyboard = new KeyboardBuilder();
            buildAction(keyboard);
            keyboard.UpdateInline(CommandService.UpdateKeyboardRows(keyboard.InlineRows));
            ReplyMarkup = new InlineKeyboardMarkup(keyboard.InlineRows);
            return this;
        }
        #endregion
    }
}
