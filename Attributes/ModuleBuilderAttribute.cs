using System;
using System.Collections.Generic;
using System.Text;

namespace FluentCommands.Attributes
{
    /// <summary>
    /// Declares a method as one to execute before building <see cref="Command"/> objects for the <see cref="CommandService"/>.
    /// <para>Method signature must be <c>public static void</c> with no parameters.</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class ModuleBuilderAttribute : Attribute
    {
        /// <summary>
        /// Declares a method as one to execute before building <see cref="Command"/> objects for the <see cref="CommandService"/>.
        /// </summary>
        public ModuleBuilderAttribute() { }
    }
}
