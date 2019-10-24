using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using FluentCommands.Attributes;
using FluentCommands.Exceptions;
using Xunit;
using Moq;
using Moq.Protected;
using FluentCommands.Builders;
using Telegram.Bot;
using Telegram.Bot.Args;
using System.Threading.Tasks;
using FluentCommands.Menus;

//[DeploymentItem("Microsoft.VisualStudio.TestPlatform.ObjectModel.dll")]

namespace FluentCommands.Tests.Unit
{
    public class TestModule : CommandModule<TestCommandList>
    {
        internal Action<ModuleBuilder> BuildingDelegate { get; }
        protected override void OnBuilding(ModuleBuilder moduleBuilder)
        {
            BuildingDelegate(moduleBuilder);
        }
        public TestModule() { }

        public TestModule(Action<ModuleBuilder> m) : base(m)
        {
            BuildingDelegate = m;
        }
    }
    public class TestCommandList
    {
        #region Delegates
        internal Func<TelegramBotClient, CallbackQueryEventArgs, Task> CallbackQueryCommandDelegate { get; }
        internal Func<TelegramBotClient, ChosenInlineResultEventArgs, Task> ChosenInlineResultCommandDelegate { get; }
        internal Func<TelegramBotClient, InlineQueryEventArgs, Task> InlineQueryCommandDelegate { get; }
        internal Func<TelegramBotClient, MessageEventArgs, Task> MessageCommandDelegate { get; }
        internal Func<TelegramBotClient, UpdateEventArgs, Task> UpdateCommandDelegate { get; }
        internal Func<TelegramBotClient, CallbackQueryEventArgs, Task<Menu>> CallbackQueryCommandMenuDelegate { get; }
        internal Func<TelegramBotClient, ChosenInlineResultEventArgs, Task<Menu>> ChosenInlineResultCommandMenuDelegate { get; }
        internal Func<TelegramBotClient, InlineQueryEventArgs, Task<Menu>> InlineQueryCommandMenuDelegate { get; }
        internal Func<TelegramBotClient, MessageEventArgs, Task<Menu>> MessageCommandMenuDelegate { get; }
        internal Func<TelegramBotClient, UpdateEventArgs, Task<Menu>> UpdateCommandMenuDelegate { get; }
        #endregion

        #region Ctors
        internal TestCommandList(Func<TelegramBotClient, CallbackQueryEventArgs, Task> f) => CallbackQueryCommandDelegate = f;
        internal TestCommandList(Func<TelegramBotClient, ChosenInlineResultEventArgs, Task> f) => ChosenInlineResultCommandDelegate = f;
        internal TestCommandList(Func<TelegramBotClient, InlineQueryEventArgs, Task> f) => InlineQueryCommandDelegate = f;
        internal TestCommandList(Func<TelegramBotClient, MessageEventArgs, Task> f) => MessageCommandDelegate = f;
        internal TestCommandList(Func<TelegramBotClient, UpdateEventArgs, Task> f) => UpdateCommandDelegate = f;
        internal TestCommandList(Func<TelegramBotClient, CallbackQueryEventArgs, Task<Menu>> f) => CallbackQueryCommandMenuDelegate = f;
        internal TestCommandList(Func<TelegramBotClient, ChosenInlineResultEventArgs, Task<Menu>> f) => ChosenInlineResultCommandMenuDelegate = f;
        internal TestCommandList(Func<TelegramBotClient, InlineQueryEventArgs, Task<Menu>> f) => InlineQueryCommandMenuDelegate = f;
        internal TestCommandList(Func<TelegramBotClient, MessageEventArgs, Task<Menu>> f) => MessageCommandMenuDelegate = f;
        internal TestCommandList(Func<TelegramBotClient, UpdateEventArgs, Task<Menu>> f) => UpdateCommandMenuDelegate = f;
        #endregion

        #region Command Method Signature Tests
        [Command("callback_query")]
        public async Task Test(TelegramBotClient c, CallbackQueryEventArgs e) => await CallbackQueryCommandDelegate(c, e);

        [Command("chosen_inline_result")]
        public async Task Test(TelegramBotClient c, ChosenInlineResultEventArgs e) => await ChosenInlineResultCommandDelegate(c, e);

        [Command("inline_query")]
        public async Task Test(TelegramBotClient c, InlineQueryEventArgs e) => await InlineQueryCommandDelegate(c, e);

        [Command("message")]
        public async Task Test(TelegramBotClient c, MessageEventArgs e) => await MessageCommandDelegate(c, e);

        [Command("update")]
        public async Task Test(TelegramBotClient c, UpdateEventArgs e) => await UpdateCommandDelegate(c, e);

        [Command("callback_query_menu")]
        public async Task<Menu> TestMenu(TelegramBotClient c, CallbackQueryEventArgs e) => await CallbackQueryCommandMenuDelegate(c, e);

        [Command("chosen_inline_result_menu")]
        public async Task<Menu> TestMenu(TelegramBotClient c, ChosenInlineResultEventArgs e) => await ChosenInlineResultCommandMenuDelegate(c, e);

        [Command("inline_query_menu")]
        public async Task<Menu> TestMenu(TelegramBotClient c, InlineQueryEventArgs e) => await InlineQueryCommandMenuDelegate(c, e);

        [Command("message_menu")]
        public async Task<Menu> TestMenu(TelegramBotClient c, MessageEventArgs e) => await MessageCommandMenuDelegate(c, e);

        [Command("update_menu")]
        public async Task<Menu> TestMenu(TelegramBotClient c, UpdateEventArgs e) => await UpdateCommandMenuDelegate(c, e);
        #endregion

    }
    public class CommandServiceTests
    {
        private static readonly Type[] _telegramEventArgs = {
            typeof(CallbackQueryEventArgs),
            typeof(ChosenInlineResultEventArgs),
            typeof(InlineQueryEventArgs),
            typeof(MessageEventArgs),
            typeof(UpdateEventArgs)
        };


        [Fact]
        public void Module_EmptyModuleConditionsDoNotThrow()
        {
            CommandService.Module<TestCommandList>(m => { });

            // No assert needed
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("/")]
        [InlineData("}")]
        [InlineData("testing spaces")]
        [InlineData("testing[symbols")]
        [InlineData("testing-dashes")]
        public void Init_1_ErroneousCommandNamesThrow(string commandName)
        {
            Assert.Throws<CommandOnBuildingException>(() => 
            {
                CommandService.Module<TestCommandList>(m =>
                {
                    m[commandName].Aliases("test");
                });

                CommandService.Start();
            });

            Assert.Throws<CommandOnBuildingException>(() =>
            {
                CommandService.Module<TestCommandList>(m =>
                {
                    m["test"].Aliases(commandName);
                });

                CommandService.Start();
            });

            Assert.Throws<CommandOnBuildingException>(() =>
            {
                CommandService.Module<TestCommandList>(m =>
                {
                    m.Command(commandName);
                });

                CommandService.Start();
            });

            Assert.Throws<CommandOnBuildingException>(() =>
            {
                CommandService.Module<TestCommandList>(m =>
                {
                    m.Command("test").Aliases(commandName);
                });

                CommandService.Start();
            });
        }

        [Fact]
        public void Module_EmptyAliasesThrow()
        {
            Assert.Throws<CommandOnBuildingException>(() =>
            {
                CommandService.Module<TestCommandList>(m =>
                {
                    m.Command("test").Aliases();
                });
            });

            Assert.Throws<CommandOnBuildingException>(() =>
            {
                CommandService.Module<TestCommandList>().Command("test").Aliases();
            });
        }

        [Fact]
        public void Module_DuplicateModulesThrow()
        {
            Assert.Throws<CommandOnBuildingException>(() =>
            {
                CommandService.Module<TestCommandList>();
                CommandService.Module<TestCommandList>();
            });
        }

        [Fact]
        public void Init_1_CommandModuleShowsCorrectGenericClassDefinition()
        {
            var expected = new TestModule().CommandClass;
            var actual = typeof(TestCommandList);

            Assert.True(expected == actual);
        }

        [Fact]
        public void ThrowsException()
        {
            Assert.ThrowsAsync<Exception>(() => { throw new Exception("lmao"); });
        }
    }
}
