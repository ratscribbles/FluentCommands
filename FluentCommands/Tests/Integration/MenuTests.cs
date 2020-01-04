using FluentCommands.Commands;
using FluentCommands.Menus;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace FluentCommands.Tests.Integration
{
    public class MenuTests
    {
        // Menu Tests
        public class MenuModesWorkProperlyForEachMenuType
        {
            #region No Action
            public class TestNoAction : CommandModule<TestNoAction>
            {
                protected override void OnConfiguring(ModuleConfigBuilder config)
                {
                    config.AddClient(Tokens.Token);
                }

                [Command("animation")]
                public async Task _1(MessageContext ctx)
                {
                    using StreamReader sr = new StreamReader(@"E:\Dropbox\FluentCommands\Tests\Source\test_video.gif");
                    await Menu.Animation(sr.BaseStream, "wow").SendAsync(ctx);
                }
                [Command("audio")]
                public async Task _2(MessageContext ctx)
                {
                    using StreamReader sr = new StreamReader(@"E:\Dropbox\FluentCommands\Tests\Source\test_audio.mp3");
                    await Menu.Audio(sr.BaseStream, "Testing").SendAsync(ctx);
                }
                [Command("contact")]
                public async Task _3(MessageContext ctx)
                {
                    await Menu.Contact("8000000000").FirstName("Testing").SendAsync(ctx);
                }
                [Command("document")]
                public async Task _4(MessageContext ctx)
                {
                    using StreamReader sr = new StreamReader(@"E:\Dropbox\FluentCommands\Tests\Source\test_image.png");
                    await Menu.Document(sr.BaseStream).SendAsync(ctx);
                }
                [Command("game")]
                public async Task _5(MessageContext ctx)
                {
                    await Menu.Game("Testing").SendAsync(ctx);
                    //! Test successful; I don't know any Game shortnames, but the correct exception is thrown when invalid.
                }
                [Command("invoice")]
                public async Task _6(MessageContext ctx)
                {
                    //await Menu.Invoice("Testing")
                    //? I don't know how to use deep-linking, lol.
                    await Menu.Text("I don't know how to test invoices.").SendAsync(ctx);
                }
                [Command("location")]
                public async Task _7(MessageContext ctx)
                {
                    await Menu.Location(1.0f, 1.0f).SendAsync(ctx);
                }
                [Command("mediagroup")]
                public async Task _8(MessageContext ctx)
                {
                    using StreamReader sr = new StreamReader(@"E:\Dropbox\FluentCommands\Tests\Source\test_image.png");
                    var photo = new InputMedia(sr.BaseStream, "test");
                    await Menu.MediaGroup(new InputMediaPhoto(photo), new InputMediaPhoto(photo)).SendAsync(ctx);
                    //: Not going through.
                }
                [Command("photo")]
                public async Task _9(MessageContext ctx)
                {
                    using StreamReader sr = new StreamReader(@"E:\Dropbox\FluentCommands\Tests\Source\test_image.png");
                    await Menu.Photo(sr.BaseStream).SendAsync(ctx);
                }
                [Command("poll")]
                public async Task _10(MessageContext ctx)
                {
                    await Menu.Poll("Does this work?").Options("Yes", "No").SendAsync(ctx);
                    //! Test successful. Polls cannot be sent to private chats.
                }
                [Command("sticker")]
                public async Task _11(MessageContext ctx)
                {
                    using StreamReader sr = new StreamReader(@"E:\Dropbox\FluentCommands\Tests\Source\test_sticker.webp");
                    await Menu.Sticker(sr.BaseStream).SendAsync(ctx);
                }
                [Command("text")]
                public async Task _12(MessageContext ctx)
                {
                    await Menu.Text("Test Successful").SendAsync(ctx);
                }
                [Command("venue")]
                public async Task _13(MessageContext ctx)
                {
                    await Menu.Venue(1.0f, 1.0f).Title("Test Venue").Address("Somewhere...").SendAsync(ctx);
                }
                [Command("video")]
                public async Task _14(MessageContext ctx)
                {
                    using StreamReader sr = new StreamReader(@"E:\Dropbox\FluentCommands\Tests\Source\test_video.mp4");
                    await Menu.Video(sr.BaseStream).SendAsync(ctx);
                    //: Sent twice.
                }
                [Command("videonote")]
                public async Task _15(MessageContext ctx)
                {
                    using StreamReader sr = new StreamReader(@"E:\Dropbox\FluentCommands\Tests\Source\test_video.mp4");
                    await Menu.VideoNote(sr.BaseStream).SendAsync(ctx);
                }
                [Command("voice")]
                public async Task _16(MessageContext ctx)
                {
                    using StreamReader sr = new StreamReader(@"E:\Dropbox\FluentCommands\Tests\Source\test_voice.ogg");
                    await Menu.Voice(sr.BaseStream).SendAsync(ctx);
                }
            }
            #endregion

            #region EditLastMessage
            public class TestEditLast : CommandModule<TestEditLast>
            {
                protected override void OnConfiguring(ModuleConfigBuilder config)
                {
                    config.AddClient(Tokens.Token2);
                    config.DefaultErrorMessageOverride("AHHHHHHHHH", Telegram.Bot.Types.Enums.ParseMode.Default);
                }

                [Command("animation")]
                public async Task _1(MessageContext ctx)
                {
                    using StreamReader sr = new StreamReader(@"E:\Dropbox\FluentCommands\Tests\Source\test_video.mp4");
                    await Menu.Animation(sr.BaseStream, "wow").SendAsync(ctx, menuModeOverride: MenuMode.EditLastMessage);
                }
                [Command("audio")]
                public async Task _2(MessageContext ctx)
                {
                    using StreamReader sr = new StreamReader(@"E:\Dropbox\FluentCommands\Tests\Source\test_audio.mp3");
                    await Menu.Audio(sr.BaseStream, "Testing").SendAsync(ctx, menuModeOverride: MenuMode.EditLastMessage);
                }
                [Command("contact")]
                public async Task _3(MessageContext ctx)
                {
                    await Menu.Contact("8000000000").FirstName("Testing").SendAsync(ctx, menuModeOverride: MenuMode.EditLastMessage);
                }
                [Command("document")]
                public async Task _4(MessageContext ctx)
                {
                    using StreamReader sr = new StreamReader(@"E:\Dropbox\FluentCommands\Tests\Source\test_image.png");
                    await Menu.Document(sr.BaseStream).SendAsync(ctx, menuModeOverride: MenuMode.EditLastMessage);
                }
                [Command("game")]
                public async Task _5(MessageContext ctx)
                {
                    await Menu.Game("Testing").SendAsync(ctx, menuModeOverride: MenuMode.EditLastMessage);
                    //! Test successful; I don't know any Game shortnames, but the correct exception is thrown when invalid.
                }
                [Command("invoice")]
                public async Task _6(MessageContext ctx)
                {
                    //await Menu.Invoice("Testing")
                    //? I don't know how to use deep-linking, lol.
                    await Menu.Text("I don't know how to test invoices.").SendAsync(ctx, menuModeOverride: MenuMode.EditLastMessage);
                }
                [Command("location")]
                public async Task _7(MessageContext ctx)
                {
                    await Menu.Location(1.0f, 1.0f).SendAsync(ctx, menuModeOverride: MenuMode.EditLastMessage);
                }
                [Command("mediagroup")]
                public async Task _8(MessageContext ctx)
                {
                    using StreamReader sr = new StreamReader(@"E:\Dropbox\FluentCommands\Tests\Source\test_image.png");
                    var photo = new InputMedia(sr.BaseStream, "test");
                    await Menu.MediaGroup(new InputMediaPhoto(photo), new InputMediaPhoto(photo)).SendAsync(ctx, menuModeOverride: MenuMode.EditLastMessage);
                    //: Not going through.
                }
                [Command("photo")]
                public async Task _9(MessageContext ctx)
                {
                    using StreamReader sr = new StreamReader(@"E:\Dropbox\FluentCommands\Tests\Source\test_image.png");
                    await Menu.Photo(sr.BaseStream).SendAsync(ctx, menuModeOverride: MenuMode.EditLastMessage);
                }
                [Command("photo2")]
                public async Task _9_2(MessageContext ctx)
                {
                    using StreamReader sr = new StreamReader(@"E:\Dropbox\FluentCommands\Tests\Source\test_image2.png");
                    await Menu.Photo(sr.BaseStream).SendAsync(ctx, menuModeOverride: MenuMode.EditLastMessage);
                }
                [Command("poll")]
                public async Task _10(MessageContext ctx)
                {
                    await Menu.Poll("Does this work?").Options("Yes", "No").SendAsync(ctx, menuModeOverride: MenuMode.EditLastMessage);
                    //! Test successful. Polls cannot be sent to private chats.
                }
                [Command("sticker")]
                public async Task _11(MessageContext ctx)
                {
                    using StreamReader sr = new StreamReader(@"E:\Dropbox\FluentCommands\Tests\Source\test_sticker.webp");
                    await Menu.Sticker(sr.BaseStream).SendAsync(ctx, menuModeOverride: MenuMode.EditLastMessage);
                }
                [Command("text")]
                public async Task _12(MessageContext ctx)
                {
                    await Menu.Text("Test Successful").SendAsync(ctx, menuModeOverride: MenuMode.EditLastMessage);
                }
                [Command("venue")]
                public async Task _13(MessageContext ctx)
                {
                    await Menu.Venue(1.0f, 1.0f).Title("Test Venue").Address("Somewhere...").SendAsync(ctx, menuModeOverride: MenuMode.EditLastMessage);
                }
                [Command("video")]
                public async Task _14(MessageContext ctx)
                {
                    using StreamReader sr = new StreamReader(@"E:\Dropbox\FluentCommands\Tests\Source\test_video.mp4");
                    await Menu.Video(sr.BaseStream).SendAsync(ctx, menuModeOverride: MenuMode.EditLastMessage);
                    //: Sent twice.
                }
                [Command("videonote")]
                public async Task _15(MessageContext ctx)
                {
                    using StreamReader sr = new StreamReader(@"E:\Dropbox\FluentCommands\Tests\Source\test_video.mp4");
                    await Menu.VideoNote(sr.BaseStream).SendAsync(ctx, menuModeOverride: MenuMode.EditLastMessage);
                }
                [Command("voice")]
                public async Task _16(MessageContext ctx)
                {
                    using StreamReader sr = new StreamReader(@"E:\Dropbox\FluentCommands\Tests\Source\test_voice.ogg");
                    await Menu.Voice(sr.BaseStream).SendAsync(ctx, menuModeOverride: MenuMode.EditLastMessage);
                }
            }

            #endregion
        }
    }
}
