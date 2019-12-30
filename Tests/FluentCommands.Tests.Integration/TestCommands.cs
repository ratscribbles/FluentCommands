using FluentCommands.Commands;
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
    public class AmbiguousCommandsTestingWithPrefixesAndClients
    {
        //public class PrefixAndClient1 : CommandModule<PrefixAndClient1>
        //{
        //    protected override void OnConfiguring(ModuleConfigBuilder config)
        //    {
        //        config.AddClient(Tokens.Token2);
        //        config.Prefix("&");
        //    }
        //    [Command("testing")] public async Task wow(MessageContext ctx) => await Menu.Text("it worked").Send(ctx);
        //}
        //public class PrefixAndClient2 : CommandModule<PrefixAndClient2>
        //{
        //    protected override void OnConfiguring(ModuleConfigBuilder config)
        //    {
        //        config.AddClient(Tokens.Token2);
        //        config.Prefix("&");
        //    }
        //    [Command("testing")] public async Task wow(MessageContext ctx) => await Menu.Text("it worked").Send(ctx);
        //}
        //public class SamePrefix1 : CommandModule<SamePrefix1>
        //{
        //    protected override void OnConfiguring(ModuleConfigBuilder config)
        //    {
        //        config.Prefix("&");
        //    }
        //    [Command("testing")] public async Task wow(MessageContext ctx) => await Menu.Text("it worked").Send(ctx);
        //}
        //public class SamePrefix2 : CommandModule<SamePrefix2>
        //{
        //    protected override void OnConfiguring(ModuleConfigBuilder config)
        //    {
        //        config.AddClient(Tokens.Token3);
        //        config.Prefix("&");
        //    }
        //    [Command("testing")] public async Task wow(MessageContext ctx) => await Menu.Text("it worked").Send(ctx);
        //}
        //public class DefaultDuplicate1 : CommandModule<DefaultDuplicate1>
        //{
        //    protected override void OnConfiguring(ModuleConfigBuilder config)
        //    {
        //    }
        //    [Command("testing")] public async Task wow(MessageContext ctx) => await Menu.Text("it worked").Send(ctx);
        //}
        //public class DefaultDuplicate2 : CommandModule<DefaultDuplicate2>
        //{
        //    protected override void OnConfiguring(ModuleConfigBuilder config)
        //    {
        //        config.AddClient(Tokens.Token3);
        //    }
        //    [Command("testing")] public async Task wow(MessageContext ctx) => await Menu.Text("it worked").Send(ctx);
        //}
    }
    public class OnlyDistinctClientsStartReceiving
    {
        //public class TestModule1 : CommandModule<TestModule1>
        //{
        //    protected override void OnConfiguring(ModuleConfigBuilder config)
        //    {
        //        config.AddClient(Tokens.Token2);
        //    }
        //    [Command("1")] public async Task wow(MessageContext ctx) => await Menu.Text("it worked").Send(ctx);
        //}
        //public class TestModule2 : CommandModule<TestModule2>
        //{
        //    protected override void OnConfiguring(ModuleConfigBuilder config)
        //    {
        //        config.AddClient(Tokens.Token2);
        //    }
        //    [Command("2")] public async Task wow(MessageContext ctx) => await Menu.Text("it worked").Send(ctx);
        //}
        //public class TestModule3 : CommandModule<TestModule3>
        //{
        //    protected override void OnConfiguring(ModuleConfigBuilder config)
        //    {
        //        config.AddClient(Tokens.Token3);
        //    }
        //    [Command("3")] public async Task wow(MessageContext ctx) => await Menu.Text("it worked").Send(ctx);
        //}
        //public class TestModule4 : CommandModule<TestModule4>
        //{
        //    protected override void OnConfiguring(ModuleConfigBuilder config)
        //    {
        //        config.AddClient(Tokens.Token3);
        //    }
        //    [Command("4")] public async Task wow(MessageContext ctx) => await Menu.Text("it worked").Send(ctx);
        //}
    }

    public class ReceiveLoggingEventsSuccessfullyOnBuilding
    {
        public class TestCommands : CommandModule<TestCommands> {[Command("1")] public async Task wow(MessageContext ctx) => await Menu.Text("it worked").Send(ctx); }
        public class TestCommands2 : CommandModule<TestCommands2> {[Command("2")] public async Task wow(MessageContext ctx) => await Menu.Text("it worked").Send(ctx); }
        public class TestCommands3 : CommandModule<TestCommands3> {[Command("3")] public async Task wow(MessageContext ctx) => await Menu.Text("it worked").Send(ctx); }
        public class TestCommands4 : CommandModule<TestCommands4> {[Command("4")] public async Task wow(MessageContext ctx) => await Menu.Text("it worked").Send(ctx); }
        public class TestCommands5 : CommandModule<TestCommands5> {[Command("5")] public async Task wow(MessageContext ctx) => await Menu.Text("it worked").Send(ctx); }
        public class TestCommands6 : CommandModule<TestCommands6> {[Command("6")] public async Task wow(MessageContext ctx) => await Menu.Text("it worked").Send(ctx); }
        public class TestCommands7 : CommandModule<TestCommands7> {[Command("7")] public async Task wow(MessageContext ctx) => await Menu.Text("it worked").Send(ctx); }
        public class TestCommands8 : CommandModule<TestCommands8> {[Command("8")] public async Task wow(MessageContext ctx) => await Menu.Text("it worked").Send(ctx); }
        public class TestCommands9 : CommandModule<TestCommands9> {[Command("9")] public async Task wow(MessageContext ctx) => await Menu.Text("it worked").Send(ctx); }
        public class TestCommands10 : CommandModule<TestCommands10> {[Command("10")] public async Task wow(MessageContext ctx) => await Menu.Text("it worked").Send(ctx); }
    }

}
