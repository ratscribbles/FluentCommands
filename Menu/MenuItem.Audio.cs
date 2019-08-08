﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using FluentCommands.Interfaces;
using FluentCommands.Interfaces.MenuBuilders;
using FluentCommands.Interfaces.MenuBuilders.AudioBuilder;

namespace FluentCommands.Menu
{
    public partial class MenuItem : IMenuAudioBuilder, IMenuAudioOptionalBuilder,
        IMenuAudioCancellationToken, IMenuAudioCaption, IMenuAudioDisableNotification, IMenuAudioDuration, IMenuAudioParseMode, IMenuAudioPerformer, IMenuAudioThumbnail
    {
        #region Required
        IMenuAudioOptionalBuilder IMenuAudioBuilder.Source(string source) { Source = source; return this; }
        IMenuAudioOptionalBuilder IMenuAudioBuilder.Source(Stream content) { Source = content; return this; }
        IMenuAudioOptionalBuilder IMenuAudioBuilder.Source(Stream content, string fileName) { Source = new InputOnlineFile(content, fileName); return this; }
        IMenuAudioOptionalBuilder IMenuAudioBuilder.Source(InputOnlineFile audio) { Source = audio; return this; }
        #endregion

        #region Optional
        IMenuAudioCancellationToken IMenuAudioOptionalBuilder.CancellationToken(CancellationToken token) { Token = token; return this; }
        IMenuAudioCaption IMenuAudioOptionalBuilder.Caption(string caption) { Caption = caption; return this; }
        IMenuAudioDisableNotification IMenuAudioOptionalBuilder.DisableNotification(bool disableNotification) { DisableNotification = disableNotification; return this; }
        IMenuAudioDuration IMenuAudioOptionalBuilder.Duration(int duration) { Duration = duration; return this; }
        IMenuAudioParseMode IMenuAudioOptionalBuilder.ParseMode(ParseMode parseMode) { ParseMode = parseMode; return this; }
        IMenuAudioPerformer IMenuAudioOptionalBuilder.Performer(string performer) { Performer = performer; return this; }
        IMenuAudioThumbnail IMenuAudioOptionalBuilder.Thumbnail(string source) { Thumbnail = source; return this; }
        IMenuAudioThumbnail IMenuAudioOptionalBuilder.Thumbnail(Stream content, string fileName) { Thumbnail = new InputMedia(content, fileName); return this; }
        IMenuAudioThumbnail IMenuAudioOptionalBuilder.Thumbnail(InputMedia thumbnail) { Thumbnail = thumbnail; return this; }
        IMenuItem IMenuAudioOptionalBuilder.Title(string title) { Title = title; return this; }
        #endregion

        #region Additional Implementations
        IMenuAudioCaption IMenuAudioCancellationToken.Caption(string caption) { Caption = caption; return this; }
        IMenuAudioDisableNotification IMenuAudioCancellationToken.DisableNotification(bool disableNotification) { DisableNotification = disableNotification; return this; }
        IMenuAudioDuration IMenuAudioCancellationToken.Duration(int duration) { Duration = duration; return this; }
        IMenuAudioParseMode IMenuAudioCancellationToken.ParseMode(ParseMode parseMode) { ParseMode = parseMode; return this; }
        IMenuAudioPerformer IMenuAudioCancellationToken.Performer(string performer) { Performer = performer; return this; }
        IMenuAudioThumbnail IMenuAudioCancellationToken.Thumbnail(string source) { Thumbnail = source; return this; }
        IMenuAudioThumbnail IMenuAudioCancellationToken.Thumbnail(Stream content, string fileName) { Thumbnail = new InputMedia(content, fileName); return this; }
        IMenuAudioThumbnail IMenuAudioCancellationToken.Thumbnail(InputMedia thumbnail) { Thumbnail = thumbnail; return this; }
        IMenuItem IMenuAudioCancellationToken.Title(string title) { Title = title; return this; }
        ////
        IMenuAudioDisableNotification IMenuAudioCaption.DisableNotification(bool disableNotification) { DisableNotification = disableNotification; return this; }
        IMenuAudioDuration IMenuAudioCaption.Duration(int duration) { Duration = duration; return this; }
        IMenuAudioParseMode IMenuAudioCaption.ParseMode(ParseMode parseMode) { ParseMode = parseMode; return this; }
        IMenuAudioPerformer IMenuAudioCaption.Performer(string performer) { Performer = performer; return this; }
        IMenuAudioThumbnail IMenuAudioCaption.Thumbnail(string source) { Thumbnail = source; return this; }
        IMenuAudioThumbnail IMenuAudioCaption.Thumbnail(Stream content, string fileName) { Thumbnail = new InputMedia(content, fileName); return this; }
        IMenuAudioThumbnail IMenuAudioCaption.Thumbnail(InputMedia thumbnail) { Thumbnail = thumbnail; return this; }
        IMenuItem IMenuAudioCaption.Title(string title) { Title = title; return this; }
        ////
        IMenuAudioDuration IMenuAudioDisableNotification.Duration(int duration) { Duration = duration; return this; }
        IMenuAudioParseMode IMenuAudioDisableNotification.ParseMode(ParseMode parseMode) { ParseMode = parseMode; return this; }
        IMenuAudioPerformer IMenuAudioDisableNotification.Performer(string performer) { Performer = performer; return this; }
        IMenuAudioThumbnail IMenuAudioDisableNotification.Thumbnail(string source) { Thumbnail = source; return this; }
        IMenuAudioThumbnail IMenuAudioDisableNotification.Thumbnail(Stream content, string fileName) { Thumbnail = new InputMedia(content, fileName); return this; }
        IMenuAudioThumbnail IMenuAudioDisableNotification.Thumbnail(InputMedia thumbnail) { Thumbnail = thumbnail; return this; }
        IMenuItem IMenuAudioDisableNotification.Title(string title) { Title = title; return this; }
        ////
        IMenuAudioParseMode IMenuAudioDuration.ParseMode(ParseMode parseMode) { ParseMode = parseMode; return this; }
        IMenuAudioPerformer IMenuAudioDuration.Performer(string performer) { Performer = performer; return this; }
        IMenuAudioThumbnail IMenuAudioDuration.Thumbnail(string source) { Thumbnail = source; return this; }
        IMenuAudioThumbnail IMenuAudioDuration.Thumbnail(Stream content, string fileName) { Thumbnail = new InputMedia(content, fileName); return this; }
        IMenuAudioThumbnail IMenuAudioDuration.Thumbnail(InputMedia thumbnail) { Thumbnail = thumbnail; return this; }
        IMenuItem IMenuAudioDuration.Title(string title) { Title = title; return this; }
        ////
        IMenuAudioPerformer IMenuAudioParseMode.Performer(string performer) { Performer = performer; return this; }
        IMenuAudioThumbnail IMenuAudioParseMode.Thumbnail(string source) { Thumbnail = source; return this; }
        IMenuAudioThumbnail IMenuAudioParseMode.Thumbnail(Stream content, string fileName) { Thumbnail = new InputMedia(content, fileName); return this; }
        IMenuAudioThumbnail IMenuAudioParseMode.Thumbnail(InputMedia thumbnail) { Thumbnail = thumbnail; return this; }
        IMenuItem IMenuAudioParseMode.Title(string title) { Title = title; return this; }
        ////
        IMenuAudioThumbnail IMenuAudioPerformer.Thumbnail(string source) { Thumbnail = source; return this; }
        IMenuAudioThumbnail IMenuAudioPerformer.Thumbnail(Stream content, string fileName) { Thumbnail = new InputMedia(content, fileName); return this; }
        IMenuAudioThumbnail IMenuAudioPerformer.Thumbnail(InputMedia thumbnail) { Thumbnail = thumbnail; return this; }
        IMenuItem IMenuAudioPerformer.Title(string title) { Title = title; return this; }
        ////
        IMenuItem IMenuAudioThumbnail.Title(string title) { Title = title; return this; }
        #endregion
    }
}
