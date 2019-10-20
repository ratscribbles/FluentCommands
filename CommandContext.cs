using System;
using System.Collections.Generic;
using System.Text;
using FluentCommands.Builders;

namespace FluentCommands
{
    public abstract class CommandContext<TModule> where TModule : class // Consider Renaming
        //! Type constraint == the class that contains the actual command implementations. document this for hte user
    {
        //: Create dictionary of type, modulebuilder.
        //: When modulebuilding all of the modules, create blank modules for each type entry.
        //: Pass the modulebuilder of that class when invoking the class's method; no return needed.

        //: EG: pass that class's Module property through that class's OnBuilding method.
        internal Type CommandClass { get; } = typeof(TModule);

        /// <summary>
        /// Builds a <see cref="Command"/> module.
        /// </summary>
        /// <param name="moduleBuilder"></param>
        protected abstract void OnBuilding(ModuleBuilder moduleBuilder);

        private protected CommandContext() { }
    }
}
