using FluentCommands.Commands;
using FluentCommands.Menus;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FluentCommands.Tests.Integration
{
    public class TestCommands : CommandModule<TestCommands> { [Command("testing")] public async Task wow(MessageContext ctx) => await Menu.Text("it worked").Send(ctx); }
    public class TestCommands2 : CommandModule<TestCommands2> {[Command("testing")] public async Task wow(MessageContext ctx) => await Menu.Text("it worked").Send(ctx); }
    public class TestCommands3 : CommandModule<TestCommands3> {[Command("testing")] public async Task wow(MessageContext ctx) => await Menu.Text("it worked").Send(ctx); }
    public class TestCommands4 : CommandModule<TestCommands4> {[Command("testing")] public async Task wow(MessageContext ctx) => await Menu.Text("it worked").Send(ctx); }
    public class TestCommands5 : CommandModule<TestCommands> {[Command("testing")] public async Task wow(MessageContext ctx) => await Menu.Text("it worked").Send(ctx); }
    public class TestCommands6 : CommandModule<TestCommands> {[Command("testing")] public async Task wow(MessageContext ctx) => await Menu.Text("it worked").Send(ctx); }
    public class TestCommands7 : CommandModule<TestCommands> {[Command("testing")] public async Task wow(MessageContext ctx) => await Menu.Text("it worked").Send(ctx); }
    public class TestCommands8 : CommandModule<TestCommands> {[Command("testing")] public async Task wow(MessageContext ctx) => await Menu.Text("it worked").Send(ctx); }
    public class TestCommands9 : CommandModule<TestCommands> {[Command("testing")] public async Task wow(MessageContext ctx) => await Menu.Text("it worked").Send(ctx); }
    public class TestCommands10 : CommandModule<TestCommands> {[Command("testing")] public async Task wow(MessageContext ctx) => await Menu.Text("it worked").Send(ctx); }

}
