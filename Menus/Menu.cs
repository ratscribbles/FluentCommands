using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentCommands;
using FluentCommands.Helper;
using FluentCommands.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace FluentCommands.Menus
{
    /// <summary>
    /// Finalized <see cref="Menus.MenuItem"/> ready to send to Telegram.
    /// </summary>
    public class Menu : IFluentInterface
    {
        internal MenuItem MenuItem { get; private set; }

        /// <summary>
        /// This class cannot be instantiated directly. Please use <see cref="MenuItem.As()"/> or <see cref="MenuItem.WithChatAction(Telegram.Bot.Types.Enums.ChatAction)"/> static builder methods.
        /// </summary>
        /// <param name="m"></param>
        internal Menu(MenuItem m) => MenuItem = m;

        /// <summary>
        /// Converts a <see cref="Menus.MenuItem"/> into a <see cref="Menu"/> object implicitly.
        /// </summary>
        /// <param name="m"></param>
        public static implicit operator Menu(MenuItem m) => new Menu(m);

        public async Task Send(TelegramBotClient client, CallbackQueryEventArgs e, IReplyMarkup replyMarkup = null, MenuMode menuMode = MenuMode.Default) =>
            await Send_Logic(client, e, replyMarkup, menuMode);
        internal async Task Send<TModule>(TelegramBotClient client, CallbackQueryEventArgs e, IReplyMarkup replyMarkup = null, MenuMode menuMode = MenuMode.Default) where TModule : CommandModule<TModule> =>
            await Send_Logic(client, e, replyMarkup, menuMode, typeof(TModule));
        public async Task Send(TelegramBotClient client, ChosenInlineResultEventArgs e, IReplyMarkup replyMarkup = null, MenuMode menuMode = MenuMode.Default) =>
            await Send_Logic(client, e, replyMarkup, menuMode);
        internal async Task Send<TModule>(TelegramBotClient client, ChosenInlineResultEventArgs e, IReplyMarkup replyMarkup = null, MenuMode menuMode = MenuMode.Default) where TModule : CommandModule<TModule> =>
            await Send_Logic(client, e, replyMarkup, menuMode, typeof(TModule));
        public async Task Send(TelegramBotClient client, InlineQueryEventArgs e, IReplyMarkup replyMarkup = null, MenuMode menuMode = MenuMode.Default) =>
            await Send_Logic(client, e, replyMarkup, menuMode);
        internal async Task Send<TModule>(TelegramBotClient client, InlineQueryEventArgs e, IReplyMarkup replyMarkup = null, MenuMode menuMode = MenuMode.Default) where TModule : CommandModule<TModule> =>
            await Send_Logic(client, e, replyMarkup, menuMode, typeof(TModule));
        public async Task Send(TelegramBotClient client, MessageEventArgs e, IReplyMarkup replyMarkup = null, MenuMode menuMode = MenuMode.Default) =>
            await Send_Logic(client, e, replyMarkup, menuMode);
        internal async Task Send<TModule>(TelegramBotClient client, MessageEventArgs e, IReplyMarkup replyMarkup = null, MenuMode menuMode = MenuMode.Default) where TModule : CommandModule<TModule> =>
            await Send_Logic(client, e, replyMarkup, menuMode, typeof(TModule));
        public async Task Send(TelegramBotClient client, UpdateEventArgs e, IReplyMarkup replyMarkup = null, MenuMode menuMode = MenuMode.Default) =>
            await Send_Logic(client, e, replyMarkup, menuMode);
        internal async Task Send<TModule>(TelegramBotClient client, UpdateEventArgs e, IReplyMarkup replyMarkup = null, MenuMode menuMode = MenuMode.Default) where TModule : CommandModule<TModule> =>
            await Send_Logic(client, e, replyMarkup, menuMode, typeof(TModule));

        private async Task Send_Logic(TelegramBotClient client, EventArgs e, IReplyMarkup replyMarkup, MenuMode menuMode = MenuMode.Default, Type module = null)
        {
            //? should this method be public? aimed at transforming MenuItems into replacements for the weird client methods
            //: additionally, please fix the signature of this method
            //? possibly duplicate this method to happen with menu items on their own, and rename this one to be "send menu internal handler" or something

            //! "SEND TO THIS" property should only accept an int or long and no enum
            //! the aim should be to only provide a SPECIFIC Id to send the menuitem to
            //! otherwise the default should ALWAYS be the chat id

            //? note, all of these things are purely for MenuItem objects. what happens within the methods that arent the returned MenuItem from the Command method are not of any concern.
            //? if the user wants to do weird junk, they can. ONLY be concerned about the RETURNED MENUITEM phase of the message sending process.


            //: Check if editable before doin anything lol


            var m = MenuItem;
            MenuMode mode;

            if (menuMode != MenuMode.Default) mode = menuMode;
            else 
            {
                if (module != null) mode = CommandService.Modules?[module]?.Config?.MenuModeOverride ?? menuMode;
                else mode = CommandService.GlobalConfig.DefaultMenuMode;
            }

            long chatId = AuxiliaryMethods.GetEventArgsChatId(e);
            if (m.SendToThis == default) m.SendToThis = chatId;

            switch (mode)
            {
                case MenuMode.NoAction:
                    await NoAction();
                    break;
                case MenuMode.EditLastMessage:
                    //await EditLastMessage();
                    //!!! FOR REPLYKEYBOARDS, YOU _CANNOT_ EDIT THE REPLYMARKUP. 
                    //! THE OPTION COULD BE TO EDIT THE INLINEMARKUP OF THE PREVIOUS MESSAGE TO BE EMPTY, AND THEN SEND THE NEW MESSAGE
                    //! ALTERNATIVELY, THERE COULD BE NO EDIT, BUT THAT SEEMS KINDA BAD
                    break;
                case MenuMode.EditOrDeleteLastMessage:
                    await EditOrDeleteLastMessage();
                    break;
                default:
                    goto case MenuMode.NoAction;
            }

            async Task NoAction()
            {
                List<Message> messages = new List<Message>();

                //? ** list of types that can only accept inlinekeyboardmarkups: ** //
                //: game, invoice, 

                switch (m.MenuType)
                {
                    case MenuType.Animation:
                        messages.Add(await client.SendAnimationAsync(m.SendToThis, m.Source, m.Duration, m.Width, m.Height, m.Thumbnail, m.Caption, m.ParseMode, m.DisableNotification, m.ReplyToMessage.MessageId, replyMarkup, m.Token));
                        break;
                    case MenuType.Audio:
                        messages.Add(await client.SendAudioAsync(m.SendToThis, m.Source, m.Caption, m.ParseMode, m.Duration, m.Performer, m.Title, m.DisableNotification, m.ReplyToMessage.MessageId, replyMarkup, m.Token, m.Thumbnail));
                        break;
                    case MenuType.Contact:
                        messages.Add(await client.SendContactAsync(m.SendToThis, m.PhoneNumber, m.FirstName, m.LastName, m.DisableNotification, m.ReplyToMessage.MessageId, replyMarkup, m.Token, m.VCard));
                        break;
                    case MenuType.Document:
                        messages.Add(await client.SendDocumentAsync(m.SendToThis, m.Source, m.Caption, m.ParseMode, m.DisableNotification, m.ReplyToMessage.MessageId, replyMarkup, m.Token, m.Thumbnail));
                        break;
                    case MenuType.Game:
                        //? How to send reply keyboard lmao
                        if (replyMarkup is InlineKeyboardMarkup || replyMarkup == null) messages.Add(await client.SendGameAsync(m.SendToThis, m.ShortName, m.DisableNotification, m.ReplyToMessage.MessageId, (InlineKeyboardMarkup)replyMarkup, m.Token));
                        else messages.Add(await client.SendGameAsync(m.SendToThis, m.ShortName, m.DisableNotification, m.ReplyToMessage.MessageId, cancellationToken: m.Token)); //? Log this?
                        break;
                    case MenuType.Invoice:
                        if (CommandService._messageUserCache[client.BotId][e.Message.Chat.Id].Chat.Type != Telegram.Bot.Types.Enums.ChatType.Private) messages.Add(new Message()); //? throw? log it? idk
                        else
                        {
                            if (replyMarkup is InlineKeyboardMarkup || replyMarkup == null)
                                messages.Add(await client.SendInvoiceAsync((int)m.SendToThis, m.Title, m.Description, m.Payload, m.ProviderToken, m.StartParameter, m.Currency, m.Prices, m.ProviderData, m.PhotoUrl, m.PhotoSize, m.PhotoWidth, m.PhotoHeight, m.NeedsName, m.NeedsPhoneNumber, m.NeedsEmail, m.NeedsShippingAddress, m.IsFlexibile, m.DisableNotification, m.ReplyToMessage.MessageId, (InlineKeyboardMarkup)replyMarkup));
                            else
                                //: Log this
                                messages.Add(await client.SendInvoiceAsync((int)m.SendToThis, m.Title, m.Description, m.Payload, m.ProviderToken, m.StartParameter, m.Currency, m.Prices, m.ProviderData, m.PhotoUrl, m.PhotoSize, m.PhotoWidth, m.PhotoHeight, m.NeedsName, m.NeedsPhoneNumber, m.NeedsEmail, m.NeedsShippingAddress, m.IsFlexibile, m.DisableNotification, m.ReplyToMessage.MessageId));
                        }
                        break;
                    case MenuType.MediaGroup:
                        messages.AddRange(await client.SendMediaGroupAsync(m.Media, m.SendToThis, m.DisableNotification, m.ReplyToMessage.MessageId, m.Token));
                        break;
                    case MenuType.Photo:
                        messages.Add(await client.SendPhotoAsync(m.SendToThis, m.Source, m.Caption, m.ParseMode, m.DisableNotification, m.ReplyToMessage.MessageId, replyMarkup, m.Token));
                        break;
                    case MenuType.Poll:
                        messages.Add(await client.SendPollAsync(m.SendToThis, m.Question, m.Options, m.DisableNotification, m.ReplyToMessage.MessageId, replyMarkup, m.Token));
                        break;
                    case MenuType.Sticker:
                        messages.Add(await client.SendStickerAsync(m.SendToThis, m.Source, m.DisableNotification, m.ReplyToMessage.MessageId, replyMarkup, m.Token));
                        break;
                    case MenuType.Text:
                        messages.Add(await client.SendTextMessageAsync(m.SendToThis, m.TextString, m.ParseMode, m.DisableWebPagePreview, m.DisableNotification, m.ReplyToMessage.MessageId, replyMarkup, m.Token));
                        break;
                    case MenuType.Venue:
                        messages.Add(await client.SendVenueAsync(m.SendToThis, m.Latitude, m.Longitude, m.Title, m.Address, m.FourSquareId, m.DisableNotification, m.ReplyToMessage.MessageId, replyMarkup, m.Token, m.FourSquareType));
                        break;
                    case MenuType.Video:
                        messages.Add(await client.SendVideoAsync(m.SendToThis, m.Source, m.Duration, m.Width, m.Height, m.Caption, m.ParseMode, m.SupportsStreaming, m.DisableNotification, m.ReplyToMessage.MessageId, replyMarkup, m.Token, m.Thumbnail));
                        break;
                    case MenuType.VideoNote:
                        messages.Add(await client.SendVideoNoteAsync(m.SendToThis, m.SourceVideoNote, m.Duration, m.Length, m.DisableNotification, m.ReplyToMessage.MessageId, replyMarkup, m.Token, m.Thumbnail));
                        break;
                    case MenuType.Voice:
                        messages.Add(await client.SendVoiceAsync(m.SendToThis, m.Source, m.Caption, m.ParseMode, m.Duration, m.DisableNotification, m.ReplyToMessage.MessageId, replyMarkup, m.Token));
                        break;
                    default:
                        return;
                }

                CommandService.UpdateBotLastMessage(client, chatId, messages.ToArray());
            }

            //async Task EditLastMessage()
            //{
            //    Message msgToEdit;

            //    switch (m.MenuType)
            //    {
            //        case MenuType.Animation:
            //            msgToEdit = _messageBotCache[client.BotId][e.Message.Chat.Id];
            //            if(replyMarkup is InlineKeyboardMarkup || replyMarkup == null)
            //            {
            //                await client.EditMessageCaptionAsync(msgToEdit.Chat.Id, msgToEdit.MessageId, m.Caption, (InlineKeyboardMarkup)replyMarkup);
            //                await client.EditMessageMediaAsync(msgToEdit.Chat.Id, msgToEdit.MessageId, new InputMediaAnimation(m.Source.Url), (InlineKeyboardMarkup)replyMarkup, m.Token);
            //            }
            //            else
            //            {
            //                //? how to send keyboard properly lmao???
            //                await client.EditMessageMediaAsync(msgToEdit.Chat.Id, msgToEdit.MessageId, new InputMediaAnimation(m.Source.Url), null, m.Token);
            //                await client.EditMessageCaptionAsync(msgToEdit.Chat.Id, msgToEdit.MessageId, m.Caption, (InlineKeyboardMarkup)replyMarkup);
            //            }
            //            break;
            //        case MenuType.Audio:
            //            await client.EditMessageMediaAsync();
            //            await client.EditMessageCaptionAsync();
            //            break;
            //        case MenuType.Contact:
            //            await NoAction();
            //            break;
            //        case MenuType.Document:
            //            await client.EditMessageMediaAsync();
            //            await client.EditMessageCaptionAsync();
            //            break;
            //        case MenuType.Game:
            //            await client.EditMessageTextAsync(msgToEdit.Chat.Id, msgToEdit.MessageId, )
            //            break;
            //        case MenuType.Invoice:
            //            await NoAction();
            //            break;
            //        case MenuType.MediaGroup:
            //            break;
            //        case MenuType.Photo:
            //            await client.EditMessageMediaAsync();
            //            await client.EditMessageCaptionAsync();
            //            break;
            //        case MenuType.Poll:
            //            await NoAction();
            //            break;
            //        case MenuType.Sticker:
            //            await NoAction();
            //            break;
            //        case MenuType.Text:
            //            await client.EditMessageTextAsync(msgToEdit.Chat.Id, msgToEdit.MessageId, )
            //            break;
            //        case MenuType.Venue:
            //            await NoAction();
            //            break;
            //        case MenuType.Video:
            //            await client.EditMessageMediaAsync();
            //            await client.EditMessageCaptionAsync();
            //            break;
            //        case MenuType.VideoNote:
            //            await NoAction();
            //            break;
            //        case MenuType.Voice:
            //            await NoAction();
            //            break;
            //    }
            //}
            async Task EditOrDeleteLastMessage()
            {
                switch (m.MenuType)
                {
                    case MenuType.Animation:
                        break;
                    case MenuType.Audio:
                        break;
                    case MenuType.Contact:
                        break;
                    case MenuType.Document:
                        break;
                    case MenuType.Game:
                        break;
                    case MenuType.Invoice:
                        break;
                    case MenuType.MediaGroup:
                        break;
                    case MenuType.Photo:
                        break;
                    case MenuType.Poll:
                        break;
                    case MenuType.Sticker:
                        break;
                    case MenuType.Text:
                        break;
                    case MenuType.Venue:
                        break;
                    case MenuType.Video:
                        break;
                    case MenuType.VideoNote:
                        break;
                    case MenuType.Voice:
                        break;
                }
            }
        }
    }
}
