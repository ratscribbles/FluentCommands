using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentCommands.Interfaces.BuilderBehaviors.ModuleBuilderBehaviors
{
    public interface IBuildAliases<TNext> : IFluentInterface where TNext : ICommandBaseBuilder
    {
        internal string[] In_Aliases { get; set; }

        TNext Aliases(params string[] aliases) { In_Aliases = aliases; return (TNext)this; }
        TNext Aliases(IEnumerable<string> aliases) { In_Aliases = aliases.ToArray(); return (TNext)this; }
    }
}
