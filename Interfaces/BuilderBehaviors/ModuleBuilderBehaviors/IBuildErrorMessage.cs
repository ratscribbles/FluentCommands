using FluentCommands.Interfaces.MenuBuilders;
using FluentCommands.Menus;
using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types.Enums;

namespace FluentCommands.Interfaces.BuilderBehaviors.ModuleBuilderBehaviors
{
    public interface IBuildErrorMessage<TNext> : IFluentInterface where TNext : ICommandBaseBuilder
    {
        internal ISendableMenu? In_ErrorMessage { get; set; }

        TNext ErrorMessage(ISendableMenu menu) { In_ErrorMessage = menu; return (TNext)this; }
        TNext ErrorMessage(string message, ParseMode parseMode = ParseMode.Default) { In_ErrorMessage = Menu.Text(message).ParseMode(parseMode); return (TNext)this; }
    }
}
