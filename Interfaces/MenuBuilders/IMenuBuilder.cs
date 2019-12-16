using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
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
using FluentCommands.Menus;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;

namespace FluentCommands.Interfaces.MenuBuilders
{
    /// <summary>
    /// Responsible for directing the construction of a valid <see cref="Menu"/>.
    /// <para>This interface is a relic of the original implementation of the static <see cref="Menu"/> fluent builder process. It remains as a reference.</para>
    /// </summary>
    [Obsolete("This interface is a relic of the original implementation of the static Menu fluent builder process. It remains as a reference.")]
    internal interface IMenuBuilder : IFluentInterface
    {
        /// <summary>
        /// Marks this <see cref="Menu"/> to send an animation file (GIF or H.264/MPEG-4 AVC video without sound).
        /// <para>Bots can currently send animation files of up to a maximum 50 MB in size.</para>
        /// </summary>
        /// <param name="source">The source URL for the file of this <see cref="Menu"/>.</param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        IMenuAnimationOptionalBuilder Animation(string source);
        /// <summary>
        /// Marks this <see cref="Menu"/> to send an animation file (GIF or H.264/MPEG-4 AVC video without sound).
        /// <para>Bots can currently send animation files of up to a maximum 50 MB in size.</para>
        /// </summary>
        /// <param name="content">The source <seealso cref="Stream"/> of this <see cref="Menu"/>.</param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        IMenuAnimationOptionalBuilder Animation(Stream content);
        /// <summary>
        /// Marks this <see cref="Menu"/> to send an animation file (GIF or H.264/MPEG-4 AVC video without sound).
        /// <para>Bots can currently send animation files of up to a maximum 50 MB in size.</para>
        /// </summary>
        /// <param name="content">The source <seealso cref="Stream"/> of this <see cref="Menu"/>.</param>
        /// <param name="fileName">The name of this source file.</param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        IMenuAnimationOptionalBuilder Animation(Stream content, string fileName);
        /// <summary>
        /// Marks this <see cref="Menu"/> to send an animation file (GIF or H.264/MPEG-4 AVC video without sound).
        /// <para>Bots can currently send animation files of up to a maximum 50 MB in size.</para>
        /// </summary>
        /// <param name="animation">The source file of this <see cref="Menu"/>.</param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        IMenuAnimationOptionalBuilder Animation(InputOnlineFile animation);
        /// <summary>
        /// Marks this <see cref="Menu"/> to send an audio file, with the Telegram client displaying it in the music player.
        /// <para>Bots can currently send audio files of up to a maximum 50 MB in size.</para>
        /// <para>Must be MP3 format.</para>
        /// </summary>
        /// <param name="source">The source URL for this file of this <see cref="Menu"/>.</param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        IMenuAudioOptionalBuilder Audio(string source);
        /// <summary>
        /// Marks this <see cref="Menu"/> to send an audio file, with the Telegram client displaying it in the music player.
        /// <para>Bots can currently send audio files of up to a maximum 50 MB in size.</para>
        /// <para>Must be MP3 format.</para>
        /// </summary>
        /// <param name="content">The source <seealso cref="Stream"/> of this <see cref="Menu"/>.</param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        IMenuAudioOptionalBuilder Audio(Stream content);
        /// <summary>
        /// Marks this <see cref="Menu"/> to send an audio file, with the Telegram client displaying it in the music player.
        /// <para>Bots can currently send audio files of up to a maximum 50 MB in size.</para>
        /// <para>Must be MP3 format.</para>
        /// </summary>
        /// <param name="content">The source <seealso cref="Stream"/> of this <see cref="Menu"/>.</param>
        /// <param name="fileName">The name of this source file.</param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        IMenuAudioOptionalBuilder Audio(Stream content, string fileName);
        /// <summary>
        /// Marks this <see cref="Menu"/> to send an audio file, with the Telegram client displaying it in the music player.
        /// <para>Bots can currently send audio files of up to a maximum 50 MB in size.</para>
        /// <para>Must be MP3 format.</para>
        /// </summary>
        /// <param name="audio">The source file of this <see cref="Menu"/>.</param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        IMenuAudioOptionalBuilder Audio(InputOnlineFile audio);
        /// <summary>
        /// Marks this <see cref="Menu"/> to send a contact.
        /// <para>Required: the Phone Number of this contact.</para>
        /// </summary>
        /// <param name="phoneNumber">The Phone Number of this contact.</param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        IMenuContactBuilderPhoneNumber Contact(string phoneNumber);
        /// <summary>
        /// Marks this <see cref="Menu"/> to send general files.
        /// <para>Bots can currently send files of any type of up to 50 MB in size.</para>
        /// </summary>
        /// <param name="source">The source URL for this file of this <see cref="Menu"/>.</param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        IMenuDocumentOptionalBuilder Document(string source);
        /// <summary>
        /// Marks this <see cref="Menu"/> to send general files.
        /// <para>Bots can currently send files of any type of up to 50 MB in size.</para>
        /// </summary>
        /// <param name="content">The source <seealso cref="Stream"/> of this <see cref="Menu"/>.</param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        IMenuDocumentOptionalBuilder Document(Stream content);
        /// <summary>
        /// Marks this <see cref="Menu"/> to send general files.
        /// <para>Bots can currently send files of any type of up to 50 MB in size.</para>
        /// </summary>
        /// <param name="content">The source <seealso cref="Stream"/> of this <see cref="Menu"/>.</param>
        /// <param name="fileName">The name of this source file.</param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        IMenuDocumentOptionalBuilder Document(Stream content, string fileName);
        /// <summary>
        /// Marks this <see cref="Menu"/> to send general files.
        /// <para>Bots can currently send files of any type of up to 50 MB in size.</para>
        /// </summary>
        /// <param name="document">The source file of this <see cref="Menu"/>.</param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        IMenuDocumentOptionalBuilder Document(InputOnlineFile document);
        /// <summary>
        /// Marks this <see cref="Menu"/> to send a game. 
        /// <para>Required: the "Short Name" for this game.</para>
        /// </summary>
        /// <param name="shortName">Serves as the unique identifier for the game. Setup your games with the Botfather.</param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        IMenuGameOptionalBuilder Game(string shortName);
        /// <summary>
        /// Marks this <see cref="Menu"/> to send an invoice.
        /// <para>Required: the title (product name) for this invoice.</para>
        /// <para>1-32 characters.</para>
        /// </summary>
        /// <param name="title">Product name, 1-32 characters.</param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        IMenuInvoiceBuilderTitle Invoice(string title);
        /// <summary>
        /// Marks this <see cref="Menu"/> to send an invoice.
        /// <para>Required: The latitude and longitude for this Location.</para>
        /// </summary>
        /// <param name="latitude"></param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        IMenuLocationOptionalBuilder Location(float latitude, float longitude);
        /// <summary>
        /// Marks this <see cref="Menu"/> to send a group of photos or videos as an album. 
        /// <para>Each file must be a photo or video.</para>
        /// </summary>
        /// <param name="media">The album to be sent.</param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        IMenuMediaGroupOptionalBuilder MediaGroup(IEnumerable<IAlbumInputMedia> media);
        /// <summary>
        /// Marks this <see cref="Menu"/> to send a group of photos or videos as an album. 
        /// <para>Each file must be a photo or video.</para>
        /// </summary>
        /// <param name="media">The album to be sent.</param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        IMenuMediaGroupOptionalBuilder MediaGroup(params IAlbumInputMedia[] media);
        /// <summary>
        /// Marks this <see cref="Menu"/> to send a photo.
        /// </summary>
        /// <param name="photo">The source file of this <see cref="Menu"/>.</param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        IMenuPhotoOptionalBuilder Photo(string source);
        /// <summary>
        /// Marks this <see cref="Menu"/> to send a photo.
        /// </summary>
        /// <param name="photo">The source file of this <see cref="Menu"/>.</param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        IMenuPhotoOptionalBuilder Photo(Stream content);
        /// <summary>
        /// Marks this <see cref="Menu"/> to send a photo.
        /// </summary>
        /// <param name="photo">The source file of this <see cref="Menu"/>.</param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        IMenuPhotoOptionalBuilder Photo(Stream content, string fileName);
        /// <summary>
        /// Marks this <see cref="Menu"/> to send a photo.
        /// </summary>
        /// <param name="photo">The source file of this <see cref="Menu"/>.</param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        IMenuPhotoOptionalBuilder Photo(InputOnlineFile photo);
        /// <summary>
        /// Marks this <see cref="Menu"/> to send a native poll.
        /// <para>Required: the Question for this poll.</para>
        /// <para>1-255 characters.</para>
        /// </summary>
        /// <param name="question"></param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        IMenuPollBuilderQuestion Poll(string question);
        /// <summary>
        /// Marks this <see cref="Menu"/> to send a static .WEBP or animated .TGS sticker.
        /// </summary>
        /// <param name="source">The source URL for the file of this <see cref="Menu"/>.</param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        IMenuStickerOptionalBuilder Sticker(string source);
        /// <summary>
        /// Marks this <see cref="Menu"/> to send a static .WEBP or animated .TGS sticker.
        /// </summary>
        /// <param name="content">The source <seealso cref="Stream"/> of this <see cref="Menu"/>.</param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        IMenuStickerOptionalBuilder Sticker(Stream content);
        /// <summary>
        /// Marks this <see cref="Menu"/> to send a static .WEBP or animated .TGS sticker.
        /// </summary>
        /// <param name="content">The source <seealso cref="Stream"/> of this <see cref="Menu"/>.</param>
        /// <param name="fileName">The name of this source file.</param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        IMenuStickerOptionalBuilder Sticker(Stream content, string fileName);
        /// <summary>
        /// Marks this <see cref="Menu"/> to send a static .WEBP or animated .TGS sticker.
        /// </summary>
        /// <param name="sticker">The source file of this <see cref="Menu"/>.</param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        IMenuStickerOptionalBuilder Sticker(InputOnlineFile sticker);
        /// <summary>
        /// Marks this <see cref="Menu"/> to send a text message.
        /// </summary>
        /// <param name="text"></param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        IMenuTextOptionalBuilder Text(string text);
        /// <summary>
        /// Marks this <see cref="Menu"/> to send information about a venue.
        /// <para>Required: The latitude and longitude for this Venue.</para>
        /// </summary>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        IMenuVenueBuilderLatLong Venue(float latitude, float longitude);
        /// <summary>
        /// Marks this <see cref="Menu"/> to send video files.
        /// <para>Telegram clients support mp4 videos (other formats may be sent as Document).</para>
        /// <para>Bots can currently send video files of up to 50 MB in size.</para>
        /// </summary>
        /// <param name="source">The source URL for the file of this <see cref="Menu"/>.</param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        IMenuVideoOptionalBuilder Video(string source);
        /// <summary>
        /// Marks this <see cref="Menu"/> to send video files.
        /// <para>Telegram clients support mp4 videos (other formats may be sent as Document).</para>
        /// <para>Bots can currently send video files of up to 50 MB in size.</para>
        /// </summary>
        /// <param name="content">The source <seealso cref="Stream"/> of this <see cref="Menu"/>.</param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        IMenuVideoOptionalBuilder Video(Stream content);
        /// <summary>
        /// Marks this <see cref="Menu"/> to send video files.
        /// <para>Telegram clients support mp4 videos (other formats may be sent as Document).</para>
        /// <para>Bots can currently send video files of up to 50 MB in size.</para>
        /// </summary>
        /// <param name="content">The source <seealso cref="Stream"/> of this <see cref="Menu"/>.</param>
        /// <param name="fileName">The name of this source file.</param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        IMenuVideoOptionalBuilder Video(Stream content, string fileName);
        /// <summary>
        /// Marks this <see cref="Menu"/> to send video files.
        /// <para>Telegram clients support mp4 videos (other formats may be sent as Document).</para>
        /// <para>Bots can currently send video files of up to 50 MB in size.</para>
        /// </summary>
        /// <param name="video">The source file of this <see cref="Menu"/>.</param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        IMenuVideoOptionalBuilder Video(InputOnlineFile video);
        /// <summary>
        /// Marks this <see cref="Menu"/> to send video messages. 
        /// <para>Your video file must be in MP4 format, maximum 1 minute in length.</para>
        /// </summary>
        /// <param name="fileId">The source Id for the file of this <see cref="Menu"/>.</param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        IMenuVideoNoteOptionalBuilder VideoNote(string fileId);
        /// <summary>
        /// Marks this <see cref="Menu"/> to send video messages. 
        /// <para>Your video file must be in MP4 format, maximum 1 minute in length.</para>
        /// </summary>
        /// <param name="content">The source <seealso cref="Stream"/> of this <see cref="Menu"/>.</param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        IMenuVideoNoteOptionalBuilder VideoNote(Stream content);
        /// <summary>
        /// Marks this <see cref="Menu"/> to send video messages. 
        /// <para>Your video file must be in MP4 format, maximum 1 minute in length.</para>
        /// </summary>
        /// <param name="content">The source <seealso cref="Stream"/> of this <see cref="Menu"/>.</param>
        /// <param name="fileName">The name of this source file.</param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        IMenuVideoNoteOptionalBuilder VideoNote(Stream content, string fileName);
        /// <summary>
        /// Marks this <see cref="Menu"/> to send video messages. 
        /// <para>Your video file must be in MP4 format, maximum 1 minute in length.</para>
        /// </summary>
        /// <param name="videoNote">The source file of this <see cref="Menu"/>.</param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        IMenuVideoNoteOptionalBuilder VideoNote(InputTelegramFile videoNote);
        /// <summary>
        /// Marks this <see cref="Menu"/> to send an audio file with the Telegram client displaying the file as a playable voice message. 
        /// <para>Your audio must be in .ogg format encoded with OPUS (other formats may be sent as Audio or Document).</para>
        /// <para>Bots can currently send voice messages of up to 50 MB in size.</para>
        /// </summary>
        /// <param name="source">The source URL for this file of this <see cref="Menu"/>.</param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        IMenuVoiceOptionalBuilder Voice(string source);
        /// <summary>
        /// Marks this <see cref="Menu"/> to send an audio file with the Telegram client displaying the file as a playable voice message. 
        /// <para>Your audio must be in .ogg format encoded with OPUS (other formats may be sent as Audio or Document).</para>
        /// <para>Bots can currently send voice messages of up to 50 MB in size.</para>
        /// </summary>
        /// <param name="content">The source <seealso cref="Stream"/> of this <see cref="Menu"/>.</param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        IMenuVoiceOptionalBuilder Voice(Stream content);
        /// <summary>
        /// Marks this <see cref="Menu"/> to send an audio file with the Telegram client displaying the file as a playable voice message. 
        /// <para>Your audio must be in .ogg format encoded with OPUS (other formats may be sent as Audio or Document).</para>
        /// <para>Bots can currently send voice messages of up to 50 MB in size.</para>
        /// </summary>
        /// <param name="content">The source <seealso cref="Stream"/> of this <see cref="Menu"/>.</param>
        /// <param name="fileName">The name of this source file.</param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        IMenuVoiceOptionalBuilder Voice(Stream content, string fileName);
        /// <summary>
        /// Marks this <see cref="Menu"/> to send an audio file with the Telegram client displaying the file as a playable voice message. 
        /// <para>Your audio must be in .ogg format encoded with OPUS (other formats may be sent as Audio or Document).</para>
        /// <para>Bots can currently send voice messages of up to 50 MB in size.</para>
        /// </summary>
        /// <param name="voice">The source file of this <see cref="Menu"/>.</param>
        /// <returns>Returns this <see cref="Menu"/> to continue fluently building its parameters.</returns>
        IMenuVoiceOptionalBuilder Voice(InputOnlineFile voice);
    }
}
