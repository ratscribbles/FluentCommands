﻿using FluentCommands.Commands;
using FluentCommands.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentCommands.Tests.Integration
{
    internal static class Tokens
    {
        internal static string Token { get; set; } = System.IO.File.ReadAllLines(@"E:\Dropbox\FluentCommands\botinf1.txt").ElementAt(0);
        internal static string Token2 { get; set; } = System.IO.File.ReadAllLines(@"E:\Dropbox\FluentCommands\botinf2.txt").ElementAt(0);
        internal static string Token3 { get; set; } = System.IO.File.ReadAllLines(@"E:\Dropbox\FluentCommands\botinf3.txt").ElementAt(0);
    }

    public class GuaranteeAmbiguousCommandsInDifferentModulesThrow
    {
        //public class TestCommands : CommandModule<TestCommands> {[Command("testing")] public async Task wow(MessageContext ctx) => await Menu.Text("it worked").Send(ctx); }
        //public class TestCommands2 : CommandModule<TestCommands2> {[Command("testing")] public async Task wow(MessageContext ctx) => await Menu.Text("it worked").Send(ctx); }
        //public class TestCommands3 : CommandModule<TestCommands3> {[Command("testing")] public async Task wow(MessageContext ctx) => await Menu.Text("it worked").Send(ctx); }
        //public class TestCommands4 : CommandModule<TestCommands4> {[Command("testbing")] public async Task wow(MessageContext ctx) => await Menu.Text("it worked").Send(ctx); }
        //public class TestCommands5 : CommandModule<TestCommands5> {[Command("testbing")] public async Task wow(MessageContext ctx) => await Menu.Text("it worked").Send(ctx); }
        //public class TestCommands6 : CommandModule<TestCommands6> {[Command("testbing")] public async Task wow(MessageContext ctx) => await Menu.Text("it worked").Send(ctx); }
        //public class TestCommands7 : CommandModule<TestCommands7> {[Command("testding")] public async Task wow(MessageContext ctx) => await Menu.Text("it worked").Send(ctx); }
        //public class TestCommands8 : CommandModule<TestCommands8> {[Command("testding")] public async Task wow(MessageContext ctx) => await Menu.Text("it worked").Send(ctx); }
        //public class TestCommands9 : CommandModule<TestCommands9> {[Command("testding")] public async Task wow(MessageContext ctx) => await Menu.Text("it worked").Send(ctx); }
        //public class TestCommands10 : CommandModule<TestCommands10> {[Command("testping")] public async Task wow(MessageContext ctx) => await Menu.Text("it worked").Send(ctx); }
    }

    public class AmbiguousCommandsTestingWithPrefixes
    {
        public class PrefixAndClient1 : CommandModule<PrefixAndClient1>
        {
            protected override void OnConfiguring(ModuleConfigBuilder config)
            {
                config.AddClient(Tokens.Token2);
                config.Prefix("&");
            }
            [Command("testing")] public async Task wow(MessageContext ctx) => await Menu.Text("it worked").Send(ctx);
        }
        public class PrefixAndClient2 : CommandModule<PrefixAndClient2>
        {
            protected override void OnConfiguring(ModuleConfigBuilder config)
            {
                config.AddClient(Tokens.Token2);
                config.Prefix("&");
            }
            [Command("testing")] public async Task wow(MessageContext ctx) => await Menu.Text("it worked").Send(ctx);
        }
        public class SamePrefix1 : CommandModule<SamePrefix1>
        {
            protected override void OnConfiguring(ModuleConfigBuilder config)
            {
                config.Prefix("&");
            }
            [Command("testing")] public async Task wow(MessageContext ctx) => await Menu.Text("it worked").Send(ctx);
        }
        public class SamePrefix2 : CommandModule<SamePrefix2>
        {
            protected override void OnConfiguring(ModuleConfigBuilder config)
            {
                config.AddClient(Tokens.Token3);
                config.Prefix("&");
            }
            [Command("testing")] public async Task wow(MessageContext ctx) => await Menu.Text("it worked").Send(ctx);
        }
        public class DefaultDuplicate1 : CommandModule<DefaultDuplicate1>
        {
            protected override void OnConfiguring(ModuleConfigBuilder config)
            {
            }
            [Command("testing")] public async Task wow(MessageContext ctx) => await Menu.Text("it worked").Send(ctx);
        }
        public class DefaultDuplicate2 : CommandModule<DefaultDuplicate2>
        {
            protected override void OnConfiguring(ModuleConfigBuilder config)
            {
                config.AddClient(Tokens.Token3);
            }
            [Command("testing")] public async Task wow(MessageContext ctx) => await Menu.Text("it worked").Send(ctx);
        }
    }
}
