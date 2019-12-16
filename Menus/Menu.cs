using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.Payments;
using Telegram.Bot.Types.ReplyMarkups;
using FluentCommands.Interfaces;
using FluentCommands.Interfaces.MenuBuilders.AnimationBuilder;
using FluentCommands.Interfaces.MenuBuilders.AudioBuilder;
using FluentCommands.Interfaces.MenuBuilders.ContactBuilder;
using FluentCommands.Interfaces.MenuBuilders.DocumentBuilder;
using FluentCommands.Interfaces.MenuBuilders.GameBuilder;
using FluentCommands.Interfaces.MenuBuilders.InvoiceBuilder;
using FluentCommands.Interfaces.MenuBuilders.LocationBuilder;
using FluentCommands.Interfaces.MenuBuilders.MediaGroupBuilder;
using FluentCommands.Interfaces.MenuBuilders.PhotoBuilder;
using FluentCommands.Interfaces.MenuBuilders.PollBuilder;
using FluentCommands.Interfaces.MenuBuilders.StickerBuilder;
using FluentCommands.Interfaces.MenuBuilders.TextBuilder;
using FluentCommands.Interfaces.MenuBuilders.VenueBuilder;
using FluentCommands.Interfaces.MenuBuilders.VideoBuilder;
using FluentCommands.Interfaces.MenuBuilders.VideoNoteBuilder;
using FluentCommands.Interfaces.MenuBuilders.VoiceBuilder;
using FluentCommands.Interfaces.MenuBuilders;
using System.Threading.Tasks;
using Telegram.Bot;
using FluentCommands.Helper;
using FluentCommands.Exceptions;

namespace FluentCommands.Menus
{
    public partial class Menu : IFluentInterface, IMenu
    {
        //// Telegram Bot Api Properties ////
        internal string? Address { get; private set; } = default;
        internal string? Caption { get; private set; } = default;
        internal string? Currency { get; private set; } = default;
        internal bool DisableNotification { get; private set; } = default;
        internal bool DisableWebPagePreview { get; private set; } = default;
        internal string? Description { get; private set; } = default;
        internal int Duration { get; private set; } = default;
        internal string? FirstName { get; private set; } = default;
        internal string? FourSquareId { get; private set; } = default;
        internal string? FourSquareType { get; private set; } = default;
        internal int Height { get; private set; } = default;
        internal bool IsFlexibile { get; private set; } = default;
        internal string? LastName { get; private set; } = default;
        internal float Latitude { get; private set; } = default;
        internal int Length { get; private set; } = default;
        internal int LivePeriod { get; private set; } = default;
        internal float Longitude { get; private set; } = default;
        internal IEnumerable<IAlbumInputMedia>? Media { get; private set; } = default;
        internal bool NeedsEmail { get; private set; } = default;
        internal bool NeedsName { get; private set; } = default;
        internal bool NeedsPhoneNumber { get; private set; } = default;
        internal bool NeedsShippingAddress { get; private set; } = default;
        internal IEnumerable<string>? Options { get; private set; } = default;
        internal ParseMode ParseMode { get; private set; } = ParseMode.Default;
        internal string? Payload { get; private set; } = default;
        internal string? Performer { get; private set; } = default;
        internal string? PhoneNumber { get; private set; } = default;
        internal int PhotoHeight { get; private set; } = default;
        internal int PhotoSize { get; private set; } = default;
        internal int PhotoWidth { get; private set; } = default;
        internal string? PhotoUrl { get; private set; } = default;
        internal IEnumerable<LabeledPrice>? Prices { get; private set; } = default;
        internal string? ProviderData { get; private set; } = default;
        internal string? ProviderToken { get; private set; } = default;
        internal string? Question { get; private set; } = default;
        internal IReplyMarkup? ReplyMarkup { get; private set; } = default;
        internal Message? ReplyToMessage { get; private set; } = default;
        internal string? ShortName { get; private set; } = default;
        internal InputOnlineFile Source { get; private set; }
        internal InputTelegramFile SourceVideoNote { get; private set; }
        internal string? StartParameter { get; private set; } = default;
        internal bool SupportsStreaming { get; private set; } = default;
        internal string? TextString { get; private set; } = default;
        internal InputMedia? Thumbnail { get; private set; } = default;
        internal string? Title { get; private set; } = default;
        internal CancellationToken Token { get; private set; } = default;
        internal string? VCard { get; private set; } = default;
        internal int Width { get; private set; } = default;

        //// MenuItem Exclusive Properties ////
        internal MenuType MenuType { get; private set; } = MenuType.None;

        //: consider removing these properties
        internal int SendToUserId { get; private set; } = default;
        internal long SendToChatId { get; private set; } = default;
        internal ChatAction? ChatAction { get; private set; } = null;
        internal int ChatActionDuration { get; private set; } = default;


        /// <summary>This class cannot be instantiated directly. Please use the static fluent builder methods.</summary>
        private Menu(MenuType menuType, InputOnlineFile source) { MenuType = menuType; Source = source; SourceVideoNote = "no content"; }
        /// <summary>This class cannot be instantiated directly. Please use the static fluent builder methods.</summary>
        private Menu(MenuType menuType, InputTelegramFile source) { MenuType = menuType; Source = "no content"; SourceVideoNote = source; }

        public async Task Send(TelegramBotClient client, int userId, ChatAction? chatAction = null, int duration = 0)
            => await Send_Logic(client, userId: userId, chatAction: chatAction, duration: duration).ConfigureAwait(false);

        public async Task Send(TelegramBotClient client, long chatId, ChatAction? chatAction = null, int duration = 0)
            => await Send_Logic(client, chatId: chatId, chatAction: chatAction, duration: duration).ConfigureAwait(false);

        public async Task Send<TModule>(TelegramBotClient client, int userId, ChatAction? chatAction = null, int duration = 0) where TModule : CommandModule<TModule>
            => await Send_Logic(client, userId: userId, chatAction: chatAction, duration: duration, moduleType: typeof(TModule)).ConfigureAwait(false);

        public async Task Send<TModule>(TelegramBotClient client, long chatId, ChatAction? chatAction = null, int duration = 0) where TModule : CommandModule<TModule>
            => await Send_Logic(client, chatId: chatId, chatAction: chatAction, duration: duration, moduleType: typeof(TModule)).ConfigureAwait(false);

        //: overloads for eventargs

        //: Consider allowing the user to register a TelegramBotClient per module
        private async Task Send_Logic(TelegramBotClient client, TelegramUpdateEventArgs? e = null, int userId = 0, long chatId = 0, ChatAction? chatAction = null, int duration = 0, Type? moduleType = null)
        {
            //? this method's signature will never be pretty 🐀

            if (Source is null || SourceVideoNote is null)
            {
                if (CommandService.GlobalConfig.SwallowCriticalExceptions) return; //: Log and return
                else throw new NullReferenceException("Menu source was null.");
            }

            ModuleConfig config = moduleType is { } && CommandService.Modules.TryGetValue(moduleType, out var module) ? module.Config : new ModuleConfig();
            MenuMode menuMode = config.MenuModeOverride.HasValue ? (MenuMode)config.MenuModeOverride : CommandService.GlobalConfig.DefaultMenuMode;
            int replyToMessageId = ReplyToMessage is { } ? ReplyToMessage.MessageId : 0;

            // Sets the ChatId to send the Menu to. Throws on failure to confirm the Id.
            if (e is { })
            {
                if (!AuxiliaryMethods.TryGetEventArgsChatId(e, out var c_id))
                {
                    if (!AuxiliaryMethods.TryGetEventArgsUserId(e, out var u_id))
                    {
                        if (CommandService.GlobalConfig.SwallowCriticalExceptions) return; //: Log & return
                        else throw new MenuInvalidSenderException("Could not find a suitable Id to send this Menu to.");
                    }
                    else chatId = u_id;
                }
                else chatId = c_id;
            }
            else
            {
                chatId = SendToChatId != 0
                    ? SendToChatId : SendToUserId != 0
                        ? SendToUserId : throw new MenuInvalidSenderException("Could not find a suitable Id to send this Menu to.");
            }

            List<Message> messages = new List<Message>();
            try
            {
                switch (menuMode)
                {
                    case MenuMode.NoAction:
                        await NoAction().ConfigureAwait(false);
                        break;
                    case MenuMode.EditLastMessage:
                        await EditLastMessage().ConfigureAwait(false);
                        break;
                    case MenuMode.EditOrDeleteLastMessage:
                        await EditOrDeleteLastMessage().ConfigureAwait(false);
                        break;
                    default:
                        goto case MenuMode.NoAction; //: Should never happen. Log if so.
                }
            }
            catch(Exception ex) { } //: log, log, log (log all of the possible exceptions)

            await CommandService.Cache.UpdateLastMessage(client, chatId, messages.ToArray()).ConfigureAwait(false);

            async Task NoAction()
            {
                //? ** list of types that can only accept inlinekeyboardmarkups: ** //
                //: game, invoice, 

                switch (MenuType)
                {
                    case MenuType.Animation:
                        messages.Add(await client.SendAnimationAsync(chatId, Source, Duration, Width, Height, Thumbnail, Caption, ParseMode, DisableNotification, replyToMessageId, ReplyMarkup, Token));
                        break;
                    case MenuType.Audio:
                        messages.Add(await client.SendAudioAsync(chatId, Source, Caption, ParseMode, Duration, Performer, Title, DisableNotification, replyToMessageId, ReplyMarkup, Token, Thumbnail));
                        break;
                    case MenuType.Contact:
                        messages.Add(await client.SendContactAsync(chatId, PhoneNumber, FirstName, LastName, DisableNotification, replyToMessageId, ReplyMarkup, Token, VCard));
                        break;
                    case MenuType.Document:
                        messages.Add(await client.SendDocumentAsync(chatId, Source, Caption, ParseMode, DisableNotification, replyToMessageId, ReplyMarkup, Token, Thumbnail));
                        break;
                    case MenuType.Game:
                        //? How to send reply keyboard lmao
                        if (ReplyMarkup is InlineKeyboardMarkup || ReplyMarkup is null) messages.Add(await client.SendGameAsync(chatId, ShortName, DisableNotification, replyToMessageId, (InlineKeyboardMarkup?)ReplyMarkup, Token));
                        else messages.Add(await client.SendGameAsync(chatId, ShortName, DisableNotification, replyToMessageId, cancellationToken: Token)); //? Log this?
                        break;
                    case MenuType.Invoice:
                        if (false /* CommandService._messageUserCache[client.BotId][e.Message.Chat.Id].Chat.Type != TelegraBot.Types.Enums.ChatType.Private*/) messages.Add(new Message()); //? throw? log it? idk
                        else
                        {
                            if (ReplyMarkup is InlineKeyboardMarkup || ReplyMarkup is null)
                                messages.Add(await client.SendInvoiceAsync((int)chatId, Title, Description, Payload, ProviderToken, StartParameter, Currency, Prices, ProviderData, PhotoUrl, PhotoSize, PhotoWidth, PhotoHeight, NeedsName, NeedsPhoneNumber, NeedsEmail, NeedsShippingAddress, IsFlexibile, DisableNotification, replyToMessageId, (InlineKeyboardMarkup?)ReplyMarkup));
                            else
                                //: Log this
                                messages.Add(await client.SendInvoiceAsync((int)chatId, Title, Description, Payload, ProviderToken, StartParameter, Currency, Prices, ProviderData, PhotoUrl, PhotoSize, PhotoWidth, PhotoHeight, NeedsName, NeedsPhoneNumber, NeedsEmail, NeedsShippingAddress, IsFlexibile, DisableNotification, replyToMessageId));
                        }
                        break;
                    case MenuType.Location:
                        messages.Add(await client.SendLocationAsync(chatId, Latitude, Longitude, LivePeriod, DisableNotification, replyToMessageId, ReplyMarkup, Token));
                        break;
                    case MenuType.MediaGroup:
                        messages.AddRange(await client.SendMediaGroupAsync(Media, chatId, DisableNotification, replyToMessageId, Token));
                        break;
                    case MenuType.Photo:
                        messages.Add(await client.SendPhotoAsync(chatId, Source, Caption, ParseMode, DisableNotification, replyToMessageId, ReplyMarkup, Token));
                        break;
                    case MenuType.Poll:
                        messages.Add(await client.SendPollAsync(chatId, Question, Options, DisableNotification, replyToMessageId, ReplyMarkup, Token));
                        break;
                    case MenuType.Sticker:
                        messages.Add(await client.SendStickerAsync(chatId, Source, DisableNotification, replyToMessageId, ReplyMarkup, Token));
                        break;
                    case MenuType.Text:
                        messages.Add(await client.SendTextMessageAsync(chatId, TextString, ParseMode, DisableWebPagePreview, DisableNotification, replyToMessageId, ReplyMarkup, Token));
                        break;
                    case MenuType.Venue:
                        messages.Add(await client.SendVenueAsync(chatId, Latitude, Longitude, Title, Address, FourSquareId, DisableNotification, replyToMessageId, ReplyMarkup, Token, FourSquareType));
                        break;
                    case MenuType.Video:
                        messages.Add(await client.SendVideoAsync(chatId, Source, Duration, Width, Height, Caption, ParseMode, SupportsStreaming, DisableNotification, replyToMessageId, ReplyMarkup, Token, Thumbnail));
                        break;
                    case MenuType.VideoNote:
                        messages.Add(await client.SendVideoNoteAsync(chatId, SourceVideoNote, Duration, Length, DisableNotification, replyToMessageId, ReplyMarkup, Token, Thumbnail));
                        break;
                    case MenuType.Voice:
                        messages.Add(await client.SendVoiceAsync(chatId, Source, Caption, ParseMode, Duration, DisableNotification, replyToMessageId, ReplyMarkup, Token));
                        break;
                    default:
                        return;
                }
            }

            async Task EditLastMessage()
            {
                Message msgToEdit;

                switch (MenuType)
                {
                    case MenuType.Animation:
                        msgToEdit = _messageBotCache[client.BotId][e.Message.Chat.Id];
                        if (replyMarkup is InlineKeyboardMarkup || replyMarkup is null)
                        {
                            await client.EditMessageCaptionAsync(msgToEdit.Chat.Id, msgToEdit.MessageId, Caption, (InlineKeyboardMarkup)replyMarkup);
                            await client.EditMessageMediaAsync(msgToEdit.Chat.Id, msgToEdit.MessageId, new InputMediaAnimation(Source.Url), (InlineKeyboardMarkup)replyMarkup, Token);
                        }
                        else
                        {
                            //? how to send keyboard properly lmao???
                            await client.EditMessageMediaAsync(msgToEdit.Chat.Id, msgToEdit.MessageId, new InputMediaAnimation(Source.Url), null, Token);
                            await client.EditMessageCaptionAsync(msgToEdit.Chat.Id, msgToEdit.MessageId, Caption, (InlineKeyboardMarkup)replyMarkup);
                        }
                        break;
                    case MenuType.Audio:
                        await client.EditMessageMediaAsync();
                        await client.EditMessageCaptionAsync();
                        break;
                    case MenuType.Contact:
                        await NoAction();
                        break;
                    case MenuType.Document:
                        await client.EditMessageMediaAsync();
                        await client.EditMessageCaptionAsync();
                        break;
                    case MenuType.Game:
                        await client.EditMessageTextAsync(msgToEdit.Chat.Id, msgToEdit.MessageId, )
                        break;
                    case MenuType.Invoice:
                        await NoAction();
                        break;
                    case MenuType.Location:
                        await NoAction();
                        break;
                    case MenuType.MediaGroup:
                        break;
                    case MenuType.Photo:
                        await client.EditMessageMediaAsync();
                        await client.EditMessageCaptionAsync();
                        break;
                    case MenuType.Poll:
                        await NoAction();
                        break;
                    case MenuType.Sticker:
                        await NoAction();
                        break;
                    case MenuType.Text:
                        await client.EditMessageTextAsync(msgToEdit.Chat.Id, msgToEdit.MessageId, )
                        break;
                    case MenuType.Venue:
                        await NoAction();
                        break;
                    case MenuType.Video:
                        await client.EditMessageMediaAsync();
                        await client.EditMessageCaptionAsync();
                        break;
                    case MenuType.VideoNote:
                        await NoAction();
                        break;
                    case MenuType.Voice:
                        await NoAction();
                        break;
                }
            }
            async Task EditOrDeleteLastMessage()
            {
                switch (MenuType)
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
                    case MenuType.Location:
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

        #region Fluent Builder Implementation
        //// Fluent Builders ////
        /// <summary>
        /// Marks this <see cref="Menu"/> to send an animation file (GIF or H.264/MPEG-4 AVC video without sound).
        /// <para>Bots can currently send animation files of up to a maximum 50 MB in size.</para>
        /// </summary>
        /// <param name="source">The source URL for the file of this <see cref="Menu"/>.</param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        public static IMenuAnimationOptionalBuilder Animation(string source) => new Menu(MenuType.Animation, source);
        /// <summary>
        /// Marks this <see cref="Menu"/> to send an animation file (GIF or H.264/MPEG-4 AVC video without sound).
        /// <para>Bots can currently send animation files of up to a maximum 50 MB in size.</para>
        /// </summary>
        /// <param name="content">The source <seealso cref="Stream"/> of this <see cref="Menu"/>.</param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        public static IMenuAnimationOptionalBuilder Animation(Stream content) => new Menu(MenuType.Animation, content);
        /// <summary>
        /// Marks this <see cref="Menu"/> to send an animation file (GIF or H.264/MPEG-4 AVC video without sound).
        /// <para>Bots can currently send animation files of up to a maximum 50 MB in size.</para>
        /// </summary>
        /// <param name="content">The source <seealso cref="Stream"/> of this <see cref="Menu"/>.</param>
        /// <param name="fileName">The name of this source file.</param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        public static IMenuAnimationOptionalBuilder Animation(Stream content, string fileName) => new Menu(MenuType.Animation, new InputOnlineFile(content, fileName));
        /// <summary>
        /// Marks this <see cref="Menu"/> to send an animation file (GIF or H.264/MPEG-4 AVC video without sound).
        /// <para>Bots can currently send animation files of up to a maximum 50 MB in size.</para>
        /// </summary>
        /// <param name="animation">The source file of this <see cref="Menu"/>.</param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        public static IMenuAnimationOptionalBuilder Animation(InputOnlineFile animation) => new Menu(MenuType.Animation, animation);
        /// <summary>
        /// Marks this <see cref="Menu"/> to send an audio file, with the Telegram client displaying it in the music player.
        /// <para>Bots can currently send audio files of up to a maximum 50 MB in size.</para>
        /// <para>Must be MP3 format.</para>
        /// </summary>
        /// <param name="source">The source URL for this file of this <see cref="Menu"/>.</param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        public static IMenuAudioOptionalBuilder Audio(string source) => new Menu(MenuType.Audio, source);
        /// <summary>
        /// Marks this <see cref="Menu"/> to send an audio file, with the Telegram client displaying it in the music player.
        /// <para>Bots can currently send audio files of up to a maximum 50 MB in size.</para>
        /// <para>Must be MP3 format.</para>
        /// </summary>
        /// <param name="content">The source <seealso cref="Stream"/> of this <see cref="Menu"/>.</param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        public static IMenuAudioOptionalBuilder Audio(Stream content) => new Menu(MenuType.Audio, content);
        /// <summary>
        /// Marks this <see cref="Menu"/> to send an audio file, with the Telegram client displaying it in the music player.
        /// <para>Bots can currently send audio files of up to a maximum 50 MB in size.</para>
        /// <para>Must be MP3 format.</para>
        /// </summary>
        /// <param name="content">The source <seealso cref="Stream"/> of this <see cref="Menu"/>.</param>
        /// <param name="fileName">The name of this source file.</param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        public static IMenuAudioOptionalBuilder Audio(Stream content, string fileName) => new Menu(MenuType.Audio, new InputOnlineFile(content, fileName));
        /// <summary>
        /// Marks this <see cref="Menu"/> to send an audio file, with the Telegram client displaying it in the music player.
        /// <para>Bots can currently send audio files of up to a maximum 50 MB in size.</para>
        /// <para>Must be MP3 format.</para>
        /// </summary>
        /// <param name="audio">The source file of this <see cref="Menu"/>.</param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        public static IMenuAudioOptionalBuilder Audio(InputOnlineFile audio) => new Menu(MenuType.Audio, audio);
        /// <summary>
        /// Marks this <see cref="Menu"/> to send a contact.
        /// <para>Required: the Phone Number of this contact.</para>
        /// </summary>
        /// <param name="phoneNumber">The Phone Number of this contact.</param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        public static IMenuContactBuilderPhoneNumber Contact(string phoneNumber) => new Menu(MenuType.Contact) { PhoneNumber = phoneNumber };
        /// <summary>
        /// Marks this <see cref="Menu"/> to send general files.
        /// <para>Bots can currently send files of any type of up to 50 MB in size.</para>
        /// </summary>
        /// <param name="source">The source URL for this file of this <see cref="Menu"/>.</param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        public static IMenuDocumentOptionalBuilder Document(string source) => new Menu(MenuType.Document, source);
        /// <summary>
        /// Marks this <see cref="Menu"/> to send general files.
        /// <para>Bots can currently send files of any type of up to 50 MB in size.</para>
        /// </summary>
        /// <param name="content">The source <seealso cref="Stream"/> of this <see cref="Menu"/>.</param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        public static IMenuDocumentOptionalBuilder Document(Stream content) => new Menu(MenuType.Document, content);
        /// <summary>
        /// Marks this <see cref="Menu"/> to send general files.
        /// <para>Bots can currently send files of any type of up to 50 MB in size.</para>
        /// </summary>
        /// <param name="content">The source <seealso cref="Stream"/> of this <see cref="Menu"/>.</param>
        /// <param name="fileName">The name of this source file.</param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        public static IMenuDocumentOptionalBuilder Document(Stream content, string fileName) => new Menu(MenuType.Document, new InputOnlineFile(content, fileName));
        /// <summary>
        /// Marks this <see cref="Menu"/> to send general files.
        /// <para>Bots can currently send files of any type of up to 50 MB in size.</para>
        /// </summary>
        /// <param name="document">The source file of this <see cref="Menu"/>.</param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        public static IMenuDocumentOptionalBuilder Document(InputOnlineFile document) => new Menu(MenuType.Document, document);
        /// <summary>
        /// Marks this <see cref="Menu"/> to send a game. 
        /// <para>Required: the "Short Name" for this game.</para>
        /// </summary>
        /// <param name="shortName">Serves as the unique identifier for the game. Setup your games with the Botfather.</param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        public static IMenuGameOptionalBuilder Game(string shortName) => new Menu(MenuType.Game) { ShortName = shortName };
        /// <summary>
        /// Marks this <see cref="Menu"/> to send an invoice.
        /// <para>Required: the title (product name) for this invoice.</para>
        /// <para>1-32 characters.</para>
        /// </summary>
        /// <param name="title">Product name, 1-32 characters.</param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        public static IMenuInvoiceBuilderTitle Invoice(string title) => new Menu(MenuType.Invoice) { Title = title };
        /// <summary>
        /// Marks this <see cref="Menu"/> to send an invoice.
        /// <para>Required: The latitude and longitude for this Location.</para>
        /// </summary>
        /// <param name="latitude"></param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        public static IMenuLocationOptionalBuilder Location(float latitude, float longitude) => new Menu(MenuType.Location) { Latitude = latitude, Longitude = longitude };
        /// <summary>
        /// Marks this <see cref="Menu"/> to send a group of photos or videos as an album. 
        /// <para>Each file must be a photo or video.</para>
        /// </summary>
        /// <param name="media">The album to be sent.</param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        public static IMenuMediaGroupOptionalBuilder MediaGroup(IEnumerable<IAlbumInputMedia> media) => new Menu(MenuType.MediaGroup) { Media = media };
        /// <summary>
        /// Marks this <see cref="Menu"/> to send a group of photos or videos as an album. 
        /// <para>Each file must be a photo or video.</para>
        /// </summary>
        /// <param name="media">The album to be sent.</param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        public static IMenuMediaGroupOptionalBuilder MediaGroup(params IAlbumInputMedia[] media) => new Menu(MenuType.MediaGroup) { Media = media };
        /// <summary>
        /// Marks this <see cref="Menu"/> to send a photo.
        /// </summary>
        /// <param name="photo">The source file of this <see cref="Menu"/>.</param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        public static IMenuPhotoOptionalBuilder Photo(string source) => new Menu(MenuType.Photo, source);
        /// <summary>
        /// Marks this <see cref="Menu"/> to send a photo.
        /// </summary>
        /// <param name="photo">The source file of this <see cref="Menu"/>.</param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        public static IMenuPhotoOptionalBuilder Photo(Stream content) => new Menu(MenuType.Photo, content);
        /// <summary>
        /// Marks this <see cref="Menu"/> to send a photo.
        /// </summary>
        /// <param name="photo">The source file of this <see cref="Menu"/>.</param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        public static IMenuPhotoOptionalBuilder Photo(Stream content, string fileName) => new Menu(MenuType.Photo, new InputOnlineFile(content, fileName));
        /// <summary>
        /// Marks this <see cref="Menu"/> to send a photo.
        /// </summary>
        /// <param name="photo">The source file of this <see cref="Menu"/>.</param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        public static IMenuPhotoOptionalBuilder Photo(InputOnlineFile photo) => new Menu(MenuType.Photo, photo);
        /// <summary>
        /// Marks this <see cref="Menu"/> to send a native poll.
        /// <para>Required: the Question for this poll.</para>
        /// <para>1-255 characters.</para>
        /// </summary>
        /// <param name="question"></param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        public static IMenuPollBuilderQuestion Poll(string question) => new Menu(MenuType.Poll) { Question = question };
        /// <summary>
        /// Marks this <see cref="Menu"/> to send a static .WEBP or animated .TGS sticker.
        /// </summary>
        /// <param name="source">The source URL for the file of this <see cref="Menu"/>.</param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        public static IMenuStickerOptionalBuilder Sticker(string source) => new Menu(MenuType.Sticker, source);
        /// <summary>
        /// Marks this <see cref="Menu"/> to send a static .WEBP or animated .TGS sticker.
        /// </summary>
        /// <param name="content">The source <seealso cref="Stream"/> of this <see cref="Menu"/>.</param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        public static IMenuStickerOptionalBuilder Sticker(Stream content) => new Menu(MenuType.Sticker, content);
        /// <summary>
        /// Marks this <see cref="Menu"/> to send a static .WEBP or animated .TGS sticker.
        /// </summary>
        /// <param name="content">The source <seealso cref="Stream"/> of this <see cref="Menu"/>.</param>
        /// <param name="fileName">The name of this source file.</param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        public static IMenuStickerOptionalBuilder Sticker(Stream content, string fileName) => new Menu(MenuType.Sticker, new InputOnlineFile(content, fileName));
        /// <summary>
        /// Marks this <see cref="Menu"/> to send a static .WEBP or animated .TGS sticker.
        /// </summary>
        /// <param name="sticker">The source file of this <see cref="Menu"/>.</param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        public static IMenuStickerOptionalBuilder Sticker(InputOnlineFile sticker) => new Menu(MenuType.Sticker, sticker);
        /// <summary>
        /// Marks this <see cref="Menu"/> to send a text message.
        /// </summary>
        /// <param name="text"></param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        public static IMenuTextOptionalBuilder Text(string text) => new Menu(MenuType.Text) { TextString = text };
        /// <summary>
        /// Marks this <see cref="Menu"/> to send information about a venue.
        /// <para>Required: The latitude and longitude for this Venue.</para>
        /// </summary>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        public static IMenuVenueBuilderLatLong Venue(float latitude, float longitude) => new Menu(MenuType.Venue) { Latitude = latitude, Longitude = longitude };
        /// <summary>
        /// Marks this <see cref="Menu"/> to send video files.
        /// <para>Telegram clients support mp4 videos (other formats may be sent as Document).</para>
        /// <para>Bots can currently send video files of up to 50 MB in size.</para>
        /// </summary>
        /// <param name="source">The source URL for the file of this <see cref="Menu"/>.</param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        public static IMenuVideoOptionalBuilder Video(string source) => new Menu(MenuType.Video, source);
        /// <summary>
        /// Marks this <see cref="Menu"/> to send video files.
        /// <para>Telegram clients support mp4 videos (other formats may be sent as Document).</para>
        /// <para>Bots can currently send video files of up to 50 MB in size.</para>
        /// </summary>
        /// <param name="content">The source <seealso cref="Stream"/> of this <see cref="Menu"/>.</param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        public static IMenuVideoOptionalBuilder Video(Stream content) => new Menu(MenuType.Video, content);
        /// <summary>
        /// Marks this <see cref="Menu"/> to send video files.
        /// <para>Telegram clients support mp4 videos (other formats may be sent as Document).</para>
        /// <para>Bots can currently send video files of up to 50 MB in size.</para>
        /// </summary>
        /// <param name="content">The source <seealso cref="Stream"/> of this <see cref="Menu"/>.</param>
        /// <param name="fileName">The name of this source file.</param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        public static IMenuVideoOptionalBuilder Video(Stream content, string fileName) => new Menu(MenuType.Video, new InputOnlineFile(content, fileName));
        /// <summary>
        /// Marks this <see cref="Menu"/> to send video files.
        /// <para>Telegram clients support mp4 videos (other formats may be sent as Document).</para>
        /// <para>Bots can currently send video files of up to 50 MB in size.</para>
        /// </summary>
        /// <param name="video">The source file of this <see cref="Menu"/>.</param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        public static IMenuVideoOptionalBuilder Video(InputOnlineFile video) => new Menu(MenuType.Video, video);
        /// <summary>
        /// Marks this <see cref="Menu"/> to send video messages. 
        /// <para>Your video file must be in MP4 format, maximum 1 minute in length.</para>
        /// </summary>
        /// <param name="fileId">The source Id for the file of this <see cref="Menu"/>.</param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        public static IMenuVideoNoteOptionalBuilder VideoNote(string fileId) => new Menu(MenuType.VideoNote, new InputTelegramFile(fileId));
        /// <summary>
        /// Marks this <see cref="Menu"/> to send video messages. 
        /// <para>Your video file must be in MP4 format, maximum 1 minute in length.</para>
        /// </summary>
        /// <param name="content">The source <seealso cref="Stream"/> of this <see cref="Menu"/>.</param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        public static IMenuVideoNoteOptionalBuilder VideoNote(Stream content) => new Menu(MenuType.VideoNote, new InputTelegramFile(content));
        /// <summary>
        /// Marks this <see cref="Menu"/> to send video messages. 
        /// <para>Your video file must be in MP4 format, maximum 1 minute in length.</para>
        /// </summary>
        /// <param name="content">The source <seealso cref="Stream"/> of this <see cref="Menu"/>.</param>
        /// <param name="fileName">The name of this source file.</param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        public static IMenuVideoNoteOptionalBuilder VideoNote(Stream content, string fileName) => new Menu(MenuType.VideoNote, new InputTelegramFile(content, fileName));
        /// <summary>
        /// Marks this <see cref="Menu"/> to send video messages. 
        /// <para>Your video file must be in MP4 format, maximum 1 minute in length.</para>
        /// </summary>
        /// <param name="videoNote">The source file of this <see cref="Menu"/>.</param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        public static IMenuVideoNoteOptionalBuilder VideoNote(InputTelegramFile videoNote) => new Menu(MenuType.VideoNote, videoNote);
        /// <summary>
        /// Marks this <see cref="Menu"/> to send an audio file with the Telegram client displaying the file as a playable voice message. 
        /// <para>Your audio must be in .ogg format encoded with OPUS (other formats may be sent as Audio or Document).</para>
        /// <para>Bots can currently send voice messages of up to 50 MB in size.</para>
        /// </summary>
        /// <param name="source">The source URL for this file of this <see cref="Menu"/>.</param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        public static IMenuVoiceOptionalBuilder Voice(string source) => new Menu(MenuType.Voice, source);
        /// <summary>
        /// Marks this <see cref="Menu"/> to send an audio file with the Telegram client displaying the file as a playable voice message. 
        /// <para>Your audio must be in .ogg format encoded with OPUS (other formats may be sent as Audio or Document).</para>
        /// <para>Bots can currently send voice messages of up to 50 MB in size.</para>
        /// </summary>
        /// <param name="content">The source <seealso cref="Stream"/> of this <see cref="Menu"/>.</param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        public static IMenuVoiceOptionalBuilder Voice(Stream content) => new Menu(MenuType.Voice, content);
        /// <summary>
        /// Marks this <see cref="Menu"/> to send an audio file with the Telegram client displaying the file as a playable voice message. 
        /// <para>Your audio must be in .ogg format encoded with OPUS (other formats may be sent as Audio or Document).</para>
        /// <para>Bots can currently send voice messages of up to 50 MB in size.</para>
        /// </summary>
        /// <param name="content">The source <seealso cref="Stream"/> of this <see cref="Menu"/>.</param>
        /// <param name="fileName">The name of this source file.</param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        public static IMenuVoiceOptionalBuilder Voice(Stream content, string fileName) => new Menu(MenuType.Voice, new InputOnlineFile(content, fileName));
        /// <summary>
        /// Marks this <see cref="Menu"/> to send an audio file with the Telegram client displaying the file as a playable voice message. 
        /// <para>Your audio must be in .ogg format encoded with OPUS (other formats may be sent as Audio or Document).</para>
        /// <para>Bots can currently send voice messages of up to 50 MB in size.</para>
        /// </summary>
        /// <param name="voice">The source file of this <see cref="Menu"/>.</param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        public static IMenuVoiceOptionalBuilder Voice(InputOnlineFile voice) => new Menu(MenuType.Voice, voice);
        #endregion
    }
}