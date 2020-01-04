using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types.ReplyMarkups;
using FluentCommands.Interfaces.KeyboardBuilders;
using Telegram.Bot.Types.Enums;
using FluentCommands.Interfaces.BuilderBehaviors.ModuleBuilderBehaviors;

namespace FluentCommands.Interfaces.BaseBuilders
{
    /// <summary>
    /// Fluent builder selection of <see cref="ICommandBaseOnBuilding"/>, with <see cref="HelpDescription(string)"/> being the first available option in Intellisense.
    /// </summary>
    public interface ICommandBaseAliases : ICommandBaseBuilder, IFluentInterface,
        IBuildErrorMessage<ICommandBaseErrorMessage>, IBuildHelpDescription<ICommandBaseDescription>, ICommandBaseDescription
    {
    }
}
