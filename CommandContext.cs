using System;
using System.Collections.Generic;
using System.Text;
using FluentCommands.Builders;

namespace FluentCommands
{
    public abstract class CommandContext
    {
        //: Create dictionary of type, modulebuilder.
        //: When modulebuilding all of the modules, create blank modules for each type entry.
        //: Pass the modulebuilder of that class when invoking the class's method; no return needed.

        //! Decouple generics from this stupid fucking class please
        protected Dictionary<Type, CommandModuleBuilder> Modules { get; set; }
        protected void OnBuilding<T>(CommandModuleBuilder<T> moduleBuilder) where T : class
        {

        }
    }
}
