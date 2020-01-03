using System;
using System.Collections.Generic;
using System.Text;
using FluentCommands.Interfaces.BaseBuilders;
using FluentCommands.Interfaces.BuilderBehaviors.ModuleBuilderBehaviors;
using FluentCommands.Interfaces.KeyboardBuilders;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace FluentCommands.Interfaces.BaseBuilders
{
    /// <summary>
    /// Fluent interface responsible for easing the user through the <see cref="CommandBaseBuilder"/> construction process.
    /// </summary>
    public interface ICommandBaseOnBuilding : IFluentInterface,
        IBuildAliases<ICommandBaseAliases>, IBuildErrorMessage<ICommandBaseErrorMessage>, IBuildHelpDescription<ICommandBaseDescription>, ICommandBaseDescription
    {
    }
}
