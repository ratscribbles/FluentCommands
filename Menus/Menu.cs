using System;
using System.Collections.Generic;
using System.Linq;
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
using FluentCommands.Utility;
using FluentCommands.Exceptions;
using Telegram.Bot.Args;

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
        internal InputOnlineFile? Source { get; private set; }
        internal InputTelegramFile? SourceVideoNote { get; private set; }
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
        private Menu(MenuType menuType) { MenuType = menuType; }
        /// <summary>This class cannot be instantiated directly. Please use the static fluent builder methods.</summary>
        private Menu(MenuType menuType, InputOnlineFile source) { MenuType = menuType; Source = source; }
        /// <summary>This class cannot be instantiated directly. Please use the static fluent builder methods.</summary>
        private Menu(MenuType menuType, InputTelegramFile source) { MenuType = menuType; SourceVideoNote = source; }

        public async Task Send(TelegramBotClient client, CallbackQueryEventArgs e, ChatAction? chatAction = null, int actionDuration = 0)
            => await Send_Logic(client, e: e, chatAction: chatAction, actionDuration: actionDuration).ConfigureAwait(false);

        public async Task Send(TelegramBotClient client, MessageEventArgs e, ChatAction? chatAction = null, int actionDuration = 0)
            => await Send_Logic(client, e: e, chatAction: chatAction, actionDuration: actionDuration).ConfigureAwait(false);

        public async Task Send(TelegramBotClient client, UpdateEventArgs e, ChatAction? chatAction = null, int actionDuration = 0)
            => await Send_Logic(client, e: e, chatAction: chatAction, actionDuration: actionDuration).ConfigureAwait(false);

        public async Task Send(TelegramBotClient client, int userId, ChatAction? chatAction = null, int actionDuration = 0)
            => await Send_Logic(client, userId: userId, chatAction: chatAction, actionDuration: actionDuration).ConfigureAwait(false);

        public async Task Send(TelegramBotClient client, long chatId, ChatAction? chatAction = null, int actionDuration = 0)
            => await Send_Logic(client, chatId: chatId, chatAction: chatAction, actionDuration: actionDuration).ConfigureAwait(false);

        public async Task Send<TModule>(TelegramBotClient client, int userId, ChatAction? chatAction = null, int actionDuration = 0) where TModule : CommandModule<TModule>
            => await Send_Logic(client, userId: userId, chatAction: chatAction, actionDuration: actionDuration, moduleType: typeof(TModule)).ConfigureAwait(false);

        public async Task Send<TModule>(TelegramBotClient client, long chatId, ChatAction? chatAction = null, int actionDuration = 0) where TModule : CommandModule<TModule>
            => await Send_Logic(client, chatId: chatId, chatAction: chatAction, actionDuration: actionDuration, moduleType: typeof(TModule)).ConfigureAwait(false);

        //: overloads for eventargs

        //: Consider allowing the user to register a TelegramBotClient per module
        private async Task Send_Logic(TelegramBotClient client, TelegramUpdateEventArgs? e = null, int userId = 0, long chatId = 0, ChatAction? chatAction = null, int actionDuration = 0, Type? moduleType = null)
        {
            //? This method's signature will never be pretty.

            if (Source is null && SourceVideoNote is null)
            {
                if (CommandService.GlobalConfig.SwallowCriticalExceptions) return; //: Log and return
                else throw new NullReferenceException("Menu source was null.");
            }

            ModuleConfig config = moduleType is { } && CommandService.Modules.TryGetValue(moduleType, out var module) ? module.Config : new ModuleConfig();
            MenuMode menuMode = config.MenuModeOverride.HasValue ? config.MenuModeOverride.Value : CommandService.GlobalConfig.DefaultMenuMode;
            int replyToMessageId = ReplyToMessage is { } ? ReplyToMessage.MessageId : 0;

            // Sets the ChatId to send the Menu to. Throws on failure to confirm the Id.
            long sendTo;
            if (e is { })
            {
                if (!AuxiliaryMethods.TryGetEventArgsChatId(e, out var c_id))
                {
                    if (!AuxiliaryMethods.TryGetEventArgsUserId(e, out var u_id))
                    {
                        if (CommandService.GlobalConfig.SwallowCriticalExceptions) return; //: Log & return
                        else throw new MenuInvalidSenderException("Could not find a suitable Id to send this Menu to.");
                    }
                    else sendTo = u_id;
                }
                else sendTo = c_id;

                _ = e.TryGetUserId(out userId);
                _ = e.TryGetChatId(out chatId);
            }
            else
            {
                sendTo = SendToChatId != 0
                    ? SendToChatId : SendToUserId != 0
                        ? SendToUserId : throw new MenuInvalidSenderException("Could not find a suitable Id to send this Menu to.");

                if(chatId == 0) chatId = SendToChatId;
                if(userId == 0) userId = SendToUserId;
            }

            if (chatAction.HasValue)
            {
                await client.SendChatActionAsync(sendTo, chatAction.Value, Token);
                await Task.Delay(actionDuration);
            }

            //? menuMode = MenuMode.EditLastMessage;

            List<Message> messages = new List<Message>();
            try
            {
                switch (menuMode)
                {
                    case MenuMode.NoAction:
                        await SendMessage().ConfigureAwait(false);
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
            catch (Exception ex) { } //: log, log, log (log all of the possible exceptions)

            async Task SendMessage()
            {
                //? ** list of types that can only accept inlinekeyboardmarkups: ** //
                //: game, invoice, 

                switch (MenuType)
                {
                    case MenuType.Animation:
                        messages.Add(await client.SendAnimationAsync(sendTo, Source, Duration, Width, Height, Thumbnail, Caption, ParseMode, DisableNotification, replyToMessageId, ReplyMarkup, Token).ConfigureAwait(false));
                        break;
                    case MenuType.Audio:
                        messages.Add(await client.SendAudioAsync(sendTo, Source, Caption, ParseMode, Duration, Performer, Title, DisableNotification, replyToMessageId, ReplyMarkup, Token, Thumbnail).ConfigureAwait(false));
                        break;
                    case MenuType.Contact:
                        messages.Add(await client.SendContactAsync(sendTo, PhoneNumber, FirstName, LastName, DisableNotification, replyToMessageId, ReplyMarkup, Token, VCard).ConfigureAwait(false));
                        break;
                    case MenuType.Document:
                        messages.Add(await client.SendDocumentAsync(sendTo, Source, Caption, ParseMode, DisableNotification, replyToMessageId, ReplyMarkup, Token, Thumbnail).ConfigureAwait(false));
                        break;
                    case MenuType.Game:
                        if (ReplyMarkup is InlineKeyboardMarkup || ReplyMarkup is null) messages.Add(await client.SendGameAsync(sendTo, ShortName, DisableNotification, replyToMessageId, ReplyMarkup as InlineKeyboardMarkup, Token).ConfigureAwait(false));
                        else messages.Add(await client.SendGameAsync(sendTo, ShortName, DisableNotification, replyToMessageId, cancellationToken: Token).ConfigureAwait(false)); //: Log this?
                        break;
                    case MenuType.Invoice:
                        {
                            if (ReplyMarkup is InlineKeyboardMarkup || ReplyMarkup is null)
                                messages.Add(await client.SendInvoiceAsync((int)sendTo, Title, Description, Payload, ProviderToken, StartParameter, Currency, Prices, ProviderData, PhotoUrl, PhotoSize, PhotoWidth, PhotoHeight, NeedsName, NeedsPhoneNumber, NeedsEmail, NeedsShippingAddress, IsFlexibile, DisableNotification, replyToMessageId, ReplyMarkup as InlineKeyboardMarkup).ConfigureAwait(false));
                            else
                                //: Log this or throw
                                messages.Add(await client.SendInvoiceAsync((int)sendTo, Title, Description, Payload, ProviderToken, StartParameter, Currency, Prices, ProviderData, PhotoUrl, PhotoSize, PhotoWidth, PhotoHeight, NeedsName, NeedsPhoneNumber, NeedsEmail, NeedsShippingAddress, IsFlexibile, DisableNotification, replyToMessageId).ConfigureAwait(false));
                        }
                        break;
                    case MenuType.Location:
                        messages.Add(await client.SendLocationAsync(sendTo, Latitude, Longitude, LivePeriod, DisableNotification, replyToMessageId, ReplyMarkup, Token).ConfigureAwait(false));
                        break;
                    case MenuType.MediaGroup:
                        messages.AddRange(await client.SendMediaGroupAsync(Media, sendTo, DisableNotification, replyToMessageId, Token).ConfigureAwait(false));
                        break;
                    case MenuType.Photo:
                        messages.Add(await client.SendPhotoAsync(sendTo, Source, Caption, ParseMode, DisableNotification, replyToMessageId, ReplyMarkup, Token).ConfigureAwait(false));
                        break;
                    case MenuType.Poll:
                        messages.Add(await client.SendPollAsync(sendTo, Question, Options, DisableNotification, replyToMessageId, ReplyMarkup, Token).ConfigureAwait(false));
                        break;
                    case MenuType.Sticker:
                        messages.Add(await client.SendStickerAsync(sendTo, Source, DisableNotification, replyToMessageId, ReplyMarkup, Token).ConfigureAwait(false));
                        break;
                    case MenuType.Text:
                        messages.Add(await client.SendTextMessageAsync(sendTo, TextString, ParseMode, DisableWebPagePreview, DisableNotification, replyToMessageId, ReplyMarkup, Token).ConfigureAwait(false));
                        break;
                    case MenuType.Venue:
                        messages.Add(await client.SendVenueAsync(sendTo, Latitude, Longitude, Title, Address, FourSquareId, DisableNotification, replyToMessageId, ReplyMarkup, Token, FourSquareType).ConfigureAwait(false));
                        break;
                    case MenuType.Video:
                        messages.Add(await client.SendVideoAsync(sendTo, Source, Duration, Width, Height, Caption, ParseMode, SupportsStreaming, DisableNotification, replyToMessageId, ReplyMarkup, Token, Thumbnail).ConfigureAwait(false));
                        break;
                    case MenuType.VideoNote:
                        messages.Add(await client.SendVideoNoteAsync(sendTo, SourceVideoNote, Duration, Length, DisableNotification, replyToMessageId, ReplyMarkup, Token, Thumbnail).ConfigureAwait(false));
                        break;
                    case MenuType.Voice:
                        messages.Add(await client.SendVoiceAsync(sendTo, Source, Caption, ParseMode, Duration, DisableNotification, replyToMessageId, ReplyMarkup, Token).ConfigureAwait(false));
                        break;
                    default:
                        //: Log
                        return;
                }

                await CommandService.Cache.UpdateLastMessage(client.BotId, chatId, userId, messages.ToArray()).ConfigureAwait(false);
            }
            async Task EditLastMessage()
            {
                var msgs = await CommandService.Cache.GetMessages(client.BotId, chatId, userId);

                Message? m = msgs is { Count: 1 } ? msgs.ElementAt(0) : msgs?.LastOrDefault();

                if (m is null) { await SendMessage().ConfigureAwait(false); return; }

                switch (MenuType)
                {
                    case MenuType.Animation:
                        if (ReplyMarkup is InlineKeyboardMarkup || ReplyMarkup is null)
                        {
                            m = await client.EditMessageCaptionAsync(m.Chat.Id, m.MessageId, Caption, ReplyMarkup as InlineKeyboardMarkup, Token, ParseMode).ConfigureAwait(false);
                            m = await client.EditMessageMediaAsync(m.Chat.Id, m.MessageId, new InputMediaAnimation(Source.Url), ReplyMarkup as InlineKeyboardMarkup, Token).ConfigureAwait(false);
                            await CommandService.Cache.UpdateLastMessage(client.BotId, chatId, userId, new[] { m }).ConfigureAwait(false);
                        }
                        else { await SendMessage().ConfigureAwait(false); return; }
                        break;
                    case MenuType.Audio:
                        if (ReplyMarkup is InlineKeyboardMarkup || ReplyMarkup is null)
                        {
                            m = await client.EditMessageMediaAsync(m.Chat.Id, m.MessageId, new InputMediaAudio(Source.Url), ReplyMarkup as InlineKeyboardMarkup, Token).ConfigureAwait(false);
                            m = await client.EditMessageCaptionAsync(m.Chat.Id, m.MessageId, Caption, ReplyMarkup as InlineKeyboardMarkup, Token, ParseMode).ConfigureAwait(false);
                            await CommandService.Cache.UpdateLastMessage(client.BotId, chatId, userId, new[] { m }).ConfigureAwait(false);
                        }
                        else { await SendMessage().ConfigureAwait(false); return; }
                        break;
                    case MenuType.Contact:
                        await SendMessage().ConfigureAwait(false);
                        return;
                    case MenuType.Document:
                        if (ReplyMarkup is InlineKeyboardMarkup || ReplyMarkup is null)
                        {
                            m = await client.EditMessageMediaAsync(m.Chat.Id, m.MessageId, new InputMediaAudio(Source.Url), ReplyMarkup as InlineKeyboardMarkup, Token).ConfigureAwait(false);
                            m = await client.EditMessageCaptionAsync(m.Chat.Id, m.MessageId, Caption, ReplyMarkup as InlineKeyboardMarkup, Token, ParseMode).ConfigureAwait(false);
                            await CommandService.Cache.UpdateLastMessage(client.BotId, chatId, userId, new[] { m }).ConfigureAwait(false);
                        }
                        else { await SendMessage().ConfigureAwait(false); return; }
                        break;
                    case MenuType.Game:
                        await SendMessage().ConfigureAwait(false);
                        return;
                    case MenuType.Invoice:
                        await SendMessage().ConfigureAwait(false);
                        return;
                    case MenuType.Location:
                        await SendMessage().ConfigureAwait(false);
                        return;
                    case MenuType.MediaGroup:
                        break;
                    case MenuType.Photo:
                        if (ReplyMarkup is InlineKeyboardMarkup || ReplyMarkup is null)
                        {
                            m = await client.EditMessageMediaAsync(m.Chat.Id, m.MessageId, new InputMediaAudio(Source.Url), ReplyMarkup as InlineKeyboardMarkup, Token).ConfigureAwait(false);
                            m = await client.EditMessageCaptionAsync(m.Chat.Id, m.MessageId, Caption, ReplyMarkup as InlineKeyboardMarkup, Token, ParseMode).ConfigureAwait(false);
                            await CommandService.Cache.UpdateLastMessage(client.BotId, chatId, userId, new[] { m }).ConfigureAwait(false);
                        }
                        else { await SendMessage().ConfigureAwait(false); return; }
                        break;
                    case MenuType.Poll:
                        await SendMessage().ConfigureAwait(false);
                        return;
                    case MenuType.Sticker:
                        await SendMessage().ConfigureAwait(false);
                        return;
                    case MenuType.Text:
                        if (ReplyMarkup is InlineKeyboardMarkup || ReplyMarkup is null)
                        {
                            m = await client.EditMessageTextAsync(m.Chat.Id, m.MessageId, TextString, ParseMode, DisableWebPagePreview, ReplyMarkup as InlineKeyboardMarkup, Token).ConfigureAwait(false);
                            await CommandService.Cache.UpdateLastMessage(client.BotId, chatId, userId, new[] { m }).ConfigureAwait(false);
                        }
                        else { await SendMessage().ConfigureAwait(false); return; }
                        break;
                    case MenuType.Venue:
                        await SendMessage().ConfigureAwait(false);
                        return;
                    case MenuType.Video:
                        if (ReplyMarkup is InlineKeyboardMarkup || ReplyMarkup is null)
                        {
                            m = await client.EditMessageMediaAsync(m.Chat.Id, m.MessageId, new InputMediaVideo(Source.Url), ReplyMarkup as InlineKeyboardMarkup, Token).ConfigureAwait(false);
                            m = await client.EditMessageCaptionAsync(m.Chat.Id, m.MessageId, Caption, ReplyMarkup as InlineKeyboardMarkup, Token, ParseMode).ConfigureAwait(false);
                            await CommandService.Cache.UpdateLastMessage(client.BotId, chatId, userId, new[] { m }).ConfigureAwait(false);
                        }
                        else { await SendMessage().ConfigureAwait(false); return; }
                        break;
                    case MenuType.VideoNote:
                        await SendMessage().ConfigureAwait(false);
                        return;
                    case MenuType.Voice:
                        await SendMessage().ConfigureAwait(false);
                        return;
                }
            }
            async Task EditOrDeleteLastMessage()
            {
                var msgs = await CommandService.Cache.GetMessages(client.BotId, chatId, userId);
                if(msgs is null) { await SendMessage().ConfigureAwait(false); return; }

                Message? m = msgs is { Count: 1 } ? msgs.ElementAt(0) : msgs?.LastOrDefault();
                if (m is null) { await SendMessage().ConfigureAwait(false); return; }

                switch (MenuType)
                {
                    case MenuType.Animation:
                        if (ReplyMarkup is InlineKeyboardMarkup || ReplyMarkup is null)
                        {
                            m = await client.EditMessageCaptionAsync(m.Chat.Id, m.MessageId, Caption, ReplyMarkup as InlineKeyboardMarkup, Token, ParseMode).ConfigureAwait(false);
                            m = await client.EditMessageMediaAsync(m.Chat.Id, m.MessageId, new InputMediaAnimation(Source.Url), ReplyMarkup as InlineKeyboardMarkup, Token).ConfigureAwait(false);
                            await CommandService.Cache.UpdateLastMessage(client.BotId, chatId, userId, new[] { m }).ConfigureAwait(false);
                        }
                        else
                        {
                            await DeleteMessages().ConfigureAwait(false);
                            await SendMessage().ConfigureAwait(false);
                            return;
                        }
                        break;
                    case MenuType.Audio:
                        if (ReplyMarkup is InlineKeyboardMarkup || ReplyMarkup is null)
                        {
                            m = await client.EditMessageMediaAsync(m.Chat.Id, m.MessageId, new InputMediaAudio(Source.Url), ReplyMarkup as InlineKeyboardMarkup, Token).ConfigureAwait(false);
                            m = await client.EditMessageCaptionAsync(m.Chat.Id, m.MessageId, Caption, ReplyMarkup as InlineKeyboardMarkup, Token, ParseMode).ConfigureAwait(false);
                            await CommandService.Cache.UpdateLastMessage(client.BotId, chatId, userId, new[] { m }).ConfigureAwait(false);
                        }
                        else
                        {
                            await DeleteMessages().ConfigureAwait(false);
                            await SendMessage().ConfigureAwait(false);
                            return;
                        }
                        break;
                    case MenuType.Contact:
                        await DeleteMessages().ConfigureAwait(false);
                        await SendMessage().ConfigureAwait(false);
                        return;
                    case MenuType.Document:
                        if (ReplyMarkup is InlineKeyboardMarkup || ReplyMarkup is null)
                        {
                            m = await client.EditMessageMediaAsync(m.Chat.Id, m.MessageId, new InputMediaAudio(Source.Url), ReplyMarkup as InlineKeyboardMarkup, Token).ConfigureAwait(false);
                            m = await client.EditMessageCaptionAsync(m.Chat.Id, m.MessageId, Caption, ReplyMarkup as InlineKeyboardMarkup, Token, ParseMode).ConfigureAwait(false);
                            await CommandService.Cache.UpdateLastMessage(client.BotId, chatId, userId, new[] { m }).ConfigureAwait(false);
                        }
                        else
                        {
                            await DeleteMessages().ConfigureAwait(false);
                            await SendMessage().ConfigureAwait(false);
                            return;
                        }
                        break;
                    case MenuType.Game:
                        await DeleteMessages().ConfigureAwait(false);
                        await SendMessage().ConfigureAwait(false);
                        return;
                    case MenuType.Invoice:
                        await DeleteMessages().ConfigureAwait(false);
                        await SendMessage().ConfigureAwait(false);
                        return;
                    case MenuType.Location:
                        await DeleteMessages().ConfigureAwait(false);
                        await SendMessage().ConfigureAwait(false);
                        return;
                    case MenuType.MediaGroup:
                        await DeleteMessages().ConfigureAwait(false);
                        await SendMessage().ConfigureAwait(false);
                        return;
                    case MenuType.Photo:
                        if (ReplyMarkup is InlineKeyboardMarkup || ReplyMarkup is null)
                        {
                            m = await client.EditMessageMediaAsync(m.Chat.Id, m.MessageId, new InputMediaAudio(Source.Url), ReplyMarkup as InlineKeyboardMarkup, Token).ConfigureAwait(false);
                            m = await client.EditMessageCaptionAsync(m.Chat.Id, m.MessageId, Caption, ReplyMarkup as InlineKeyboardMarkup, Token, ParseMode).ConfigureAwait(false);
                            await CommandService.Cache.UpdateLastMessage(client.BotId, chatId, userId, new[] { m }).ConfigureAwait(false);
                        }
                        else
                        {
                            await DeleteMessages().ConfigureAwait(false);
                            await SendMessage().ConfigureAwait(false);
                            return;
                        }
                        break;
                    case MenuType.Poll:
                        await DeleteMessages().ConfigureAwait(false);
                        await SendMessage().ConfigureAwait(false);
                        return;
                    case MenuType.Sticker:
                        await DeleteMessages().ConfigureAwait(false);
                        await SendMessage().ConfigureAwait(false);
                        return;
                    case MenuType.Text:
                        if (ReplyMarkup is InlineKeyboardMarkup || ReplyMarkup is null)
                        {
                            m = await client.EditMessageTextAsync(m.Chat.Id, m.MessageId, TextString, ParseMode, DisableWebPagePreview, ReplyMarkup as InlineKeyboardMarkup, Token).ConfigureAwait(false);
                            await CommandService.Cache.UpdateLastMessage(client.BotId, chatId, userId, new[] { m }).ConfigureAwait(false);
                        }
                        else
                        {
                            await DeleteMessages().ConfigureAwait(false);
                            await SendMessage().ConfigureAwait(false);
                            return;
                        }
                        break;
                    case MenuType.Venue:
                        await DeleteMessages().ConfigureAwait(false);
                        await SendMessage().ConfigureAwait(false);
                        return;
                    case MenuType.Video:
                        if (ReplyMarkup is InlineKeyboardMarkup || ReplyMarkup is null)
                        {
                            m = await client.EditMessageMediaAsync(m.Chat.Id, m.MessageId, new InputMediaVideo(Source.Url), ReplyMarkup as InlineKeyboardMarkup, Token).ConfigureAwait(false);
                            m = await client.EditMessageCaptionAsync(m.Chat.Id, m.MessageId, Caption, ReplyMarkup as InlineKeyboardMarkup, Token, ParseMode).ConfigureAwait(false);
                            await CommandService.Cache.UpdateLastMessage(client.BotId, chatId, userId, new[] { m }).ConfigureAwait(false);
                        }
                        else
                        {
                            await DeleteMessages().ConfigureAwait(false);
                            await SendMessage().ConfigureAwait(false);
                            return;
                        }
                        break;
                    case MenuType.VideoNote:
                        await DeleteMessages().ConfigureAwait(false);
                        await SendMessage().ConfigureAwait(false);
                        return;
                    case MenuType.Voice:
                        await DeleteMessages().ConfigureAwait(false);
                        await SendMessage().ConfigureAwait(false);
                        return;
                }

                async Task DeleteMessages()
                {
                    try
                    {
                        foreach (var msg in msgs!)
                        {
                            await client.DeleteMessageAsync(msg.Chat.Id, msg.MessageId, Token);
                        }
                    }
                    catch
                    {
                        //: Log
                    }
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