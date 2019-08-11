using System;
using System.Collections.Generic;
using System.Text;
using FluentCommands.Menus;

namespace FluentCommands.Interfaces.MenuBuilders
{
    /// <summary>
    /// Responsible for directing the construction of a valid <see cref="MenuItem"/>.
    /// </summary>
    public interface IMenuBuilder : IFluentInterface
    {
        /// <summary>
        /// Marks this <see cref="MenuItem"/> to send an animation file (GIF or H.264/MPEG-4 AVC video without sound).
        /// <para>Bots can currently send animation files of up to a maximum 50 MB in size.</para>
        /// </summary>
        /// <returns>Returns this <see cref="MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuAnimationBuilder Animation();
        /// <summary>
        /// Marks this <see cref="MenuItem"/> to send an audio file, with the Telegram client displaying it in the music player.
        /// <para>Bots can currently send audio files of up to a maximum 50 MB in size.</para>
        /// <para>Must be MP3 format.</para>
        /// </summary>
        /// <returns>Returns this <see cref="MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuAudioBuilder Audio();
        /// <summary>
        /// Marks this <see cref="MenuItem"/> to send a phone contact.
        /// </summary>
        /// <returns>Returns this <see cref="MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuContactBuilder Contact();
        /// <summary>
        /// Marks this <see cref="MenuItem"/> to send general files.
        /// <para>Bots can currently send files of any type of up to 50 MB in size.</para>
        /// </summary>
        /// <returns>Returns this <see cref="MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuDocumentBuilder Document();
        /// <summary>
        /// Marks this <see cref="MenuItem"/> to send a game. 
        /// </summary>
        /// <returns>Returns this <see cref="MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuGameBuilder Game();
        /// <summary>
        /// Marks this <see cref="MenuItem"/> to send an invoice.
        /// </summary>
        /// <returns>Returns this <see cref="MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuInvoiceBuilder Invoice();
        /// <summary>
        /// Marks this <see cref="MenuItem"/> to send a group of photos or videos as an album. 
        /// </summary>
        /// <returns>Returns this <see cref="MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuMediaGroupBuilder MediaGroup();
        /// <summary>
        /// Marks this <see cref="MenuItem"/> to send photos.
        /// </summary>
        /// <returns>Returns this <see cref="MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuPhotoBuilder Photo();
        /// <summary>
        /// Marks this <see cref="MenuItem"/> to send a native poll.
        /// <para>Native polls cannot be sent to private chats.</para>
        /// </summary>
        /// <returns>Returns this <see cref="MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuPollBuilder Poll();
        /// <summary>
        /// Marks this <see cref="MenuItem"/> to send a static .WEBP or animated .TGS sticker.
        /// </summary>
        /// <returns>Returns this <see cref="MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuStickerBuilder Sticker();
        /// <summary>
        /// Marks this <see cref="MenuItem"/> to send a text message.
        /// </summary>
        /// <returns>Returns this <see cref="MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuTextBuilder Text();
        /// <summary>
        /// Marks this <see cref="MenuItem"/> to send information about a venue.
        /// </summary>
        /// <returns>Returns this <see cref="MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuVenueBuilder Venue();
        /// <summary>
        /// Marks this <see cref="MenuItem"/> to send video files.
        /// <para>Telegram clients support mp4 videos (other formats may be sent as Document).</para>
        /// <para>Bots can currently send video files of up to 50 MB in size.</para>
        /// </summary>
        /// <returns>Returns this <see cref="MenuItem"/> to continue fluently building its parameters.</returns>
        IMenuVideoBuilder Video();
        /// <summary>
        /// Marks this <see cref="MenuItem"/> to send video messages. 
        /// <para>Your video file must be in MP4 format, maximum 1 minute in length.</para>
        /// </summary>
        /// <returns></returns>
        IMenuVideoNoteBuilder VideoNote();
        /// <summary>
        /// Marks this <see cref="MenuItem"/> to send an audio file with the Telegram client displaying the file as a playable voice message. 
        /// <para>Your audio must be in .ogg format encoded with OPUS (other formats may be sent as Audio or Document).</para>
        /// <para>Bots can currently send voice messages of up to 50 MB in size.</para>
        /// </summary>
        /// <returns></returns>
        IMenuVoiceBuilder Voice();
    }
}
