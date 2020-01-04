using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types.ReplyMarkups;
using FluentCommands.Interfaces.KeyboardBuilders;
using FluentCommands.Interfaces.BuilderBehaviors.ModuleBuilderBehaviors;

namespace FluentCommands.Interfaces.BaseBuilders
{
    /// <summary>
    /// Fluent builder selection of <see cref="ICommandBaseOnBuilding"/>, with <see cref="HasKeyboard(Action{KeyboardBuilder})"/> being the first available option in Intellisense.
    /// </summary>
    public interface ICommandBaseDescription : IFluentInterface, ICommandBaseBuilder, IBuildInlineKeyboardButtonReference
    {
    }
}
