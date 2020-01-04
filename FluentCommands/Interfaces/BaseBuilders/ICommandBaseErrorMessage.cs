using FluentCommands.Interfaces.BuilderBehaviors.ModuleBuilderBehaviors;
using System;
using System.Collections.Generic;
using System.Text;

namespace FluentCommands.Interfaces.BaseBuilders
{
    public interface ICommandBaseErrorMessage : IFluentInterface, ICommandBaseBuilder,
        IBuildHelpDescription<ICommandBaseDescription>, ICommandBaseDescription
    {
    }
}
