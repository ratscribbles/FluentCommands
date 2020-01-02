using FluentCommands.Interfaces.MenuBuilders;
using FluentCommands.Menus;
using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types.Enums;

namespace FluentCommands.Interfaces.BuilderBehaviors.ModuleBuilderBehaviors
{
    public interface IBuildHelpDescription<TNext> : IFluentInterface where TNext : IModuleBuilder
    {
        internal ISendableMenu? In_HelpDescription { get; set; }

        TNext HelpDescription(ISendableMenu menu) { In_HelpDescription = menu; return (TNext)this; }
        TNext HelpDescription(string message, ParseMode parseMode = ParseMode.Default) { In_HelpDescription = Menu.Text(message).ParseMode(parseMode); return (TNext)this; }
    }
}
