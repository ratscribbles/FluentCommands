using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using FluentCommands.Attributes;
using FluentCommands.Exceptions;
using Xunit;

namespace FluentCommands.Tests.Unit
{
    public class CommandServiceTests
    {
        private class EmptyTestModule
        {
        }

        [Fact]
        public void Module_EmptyModuleConditionsDoNotThrow()
        {
            CommandService.Module<EmptyTestModule>();

            CommandService.Module<EmptyTestModule>(m => { });

            // No assert needed
        }

        //: SampleTestModule tests WIP; strategizing how to approach

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Init_ErroneousCommandNamesThrow(string commandName) => //! Currently refactoring...
            Assert.Throws<InvalidCommandNameException>(() => CommandService.Module<EmptyTestModule>().Command(commandName));
    }
}
