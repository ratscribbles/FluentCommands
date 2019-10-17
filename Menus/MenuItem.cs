using System;
using System.Collections.Generic;
using System.Text;
using FluentCommands.Interfaces;
using FluentCommands.Interfaces.MenuBuilders;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using System.Threading;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Payments;

namespace FluentCommands.Menus
{
    internal enum MenuType
    {
        Animation,
        Audio,
        Contact,
        Document,
        Game,
        Invoice,
        MediaGroup,
        None,
        Photo,
        Poll,
        Sticker,
        Text,
        Venue,
        Video,
        VideoNote,
        Voice
    }
    public partial class MenuItem : IFluentInterface, IMenuBuilder
    {
        //// Telegram Bot Api Properties ///
        internal string Address { get; private set; } = default;
        internal string Caption { get; private set; } = default;
        internal string Currency { get; private set; } = default;
        internal bool DisableNotification { get; private set; } = default;
        internal bool DisableWebPagePreview { get; private set; } = default;
        internal string Description { get; private set; } = default;
        internal int Duration { get; private set; } = default;
        internal string FirstName { get; private set; } = default;
        internal string FourSquareId { get; private set; } = default;
        internal string FourSquareType { get; private set; } = default;
        internal int Height { get; private set; } = default;
        internal bool IsFlexibile { get; private set; } = default;
        internal string LastName { get; private set; } = default;
        internal float Latitude { get; private set; } = default;
        internal int Length { get; private set; } = default;
        internal int LivePeriod { get; private set; } = default;
        internal float Longitude { get; private set; } = default;
        internal IEnumerable<IAlbumInputMedia> Media { get; private set; } = default;
        internal bool NeedsEmail { get; private set; } = default;
        internal bool NeedsName { get; private set; } = default;
        internal bool NeedsPhoneNumber { get; private set; } = default;
        internal bool NeedsShippingAddress { get; private set; } = default;
        internal IEnumerable<string> Options { get; private set; } = default;
        internal ParseMode ParseMode { get; private set; } = ParseMode.Default;
        internal string Payload { get; private set; } = default;
        internal string Performer { get; private set; } = default;
        internal string PhoneNumber { get; private set; } = default;
        internal int PhotoHeight { get; private set; } = default;
        internal int PhotoSize { get; private set; } = default;
        internal int PhotoWidth { get; private set; } = default;
        internal string PhotoUrl { get; private set; } = default;
        internal IEnumerable<LabeledPrice> Prices { get; private set; } = default;
        internal string ProviderData { get; private set; } = default;
        internal string ProviderToken { get; private set; } = default;
        internal string Question { get; private set; } = default;
        internal Message ReplyToMessage { get; private set; } = default;
        internal string ShortName { get; private set; } = default;
        internal InputOnlineFile Source { get; private set; } = default;
        internal InputTelegramFile SourceVideoNote { get; private set; } = default;
        internal string StartParameter { get; private set; } = default;
        internal bool SupportsStreaming { get; private set; } = default;
        internal string TextString { get; private set; } = default;
        internal InputMedia Thumbnail { get; private set; } = default;
        internal string Title { get; private set; } = default;
        internal CancellationToken Token { get; private set; } = default;
        internal string VCard { get; private set; } = default;
        internal int Width { get; private set; } = default;

        //// MenuItem Exclusive Properties ////
        internal long SendToThis { get; set; } = default; // Caution!! Internal setter for the SendMenu() method.
        internal MenuType MenuType { get; private set; } = MenuType.None;
        internal ChatAction? ChatAction { get; private set; } = null;

        /// <summary>
        /// This class cannot be instantiated directly. Please use <see cref="As()"/> or <see cref="WithChatAction(ChatAction)"/> static builder methods.
        /// </summary>
        private MenuItem() { }

        //// Fluent Builders ////

        internal static Menu Empty() => new Menu(new MenuItem());
        public static IMenuBuilder WithChatAction(ChatAction chatAction) { var m = new MenuItem { ChatAction = chatAction }; return m; }
        public static IMenuBuilder As() => new MenuItem();
        public IMenuAnimationBuilder Animation() { MenuType = MenuType.Animation; return this; }
        public IMenuAudioBuilder Audio() { MenuType = MenuType.Audio; return this; }
        public IMenuContactBuilder Contact() { MenuType = MenuType.Contact; return this; }
        public IMenuDocumentBuilder Document() { MenuType = MenuType.Document; return this; }
        public IMenuGameBuilder Game() { MenuType = MenuType.Game; return this; }
        public IMenuInvoiceBuilder Invoice() { MenuType = MenuType.Invoice; return this; }
        public IMenuMediaGroupBuilder MediaGroup() { MenuType = MenuType.MediaGroup; return this; }
        public IMenuPhotoBuilder Photo() { MenuType = MenuType.Photo; return this; }
        public IMenuPollBuilder Poll() { MenuType = MenuType.Poll; return this; }
        public IMenuStickerBuilder Sticker() { MenuType = MenuType.Sticker; return this; }
        public IMenuTextBuilder Text() { MenuType = MenuType.Text; return this; }
        public IMenuVenueBuilder Venue() { MenuType = MenuType.Venue; return this; }
        public IMenuVideoBuilder Video() { MenuType = MenuType.Video; return this; }
        public IMenuVideoNoteBuilder VideoNote() { MenuType = MenuType.VideoNote; return this; }
        public IMenuVoiceBuilder Voice() { MenuType = MenuType.Voice; return this; }
        public Menu Done() => new Menu(this);
        public Menu DoneAndSendTo(long idToSendTo) { SendToThis = idToSendTo; return new Menu(this); }
    }
}