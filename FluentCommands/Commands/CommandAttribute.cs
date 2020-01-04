using System;
using System.Collections.Generic;
using System.Text;
using FluentCommands.Utility;
using FluentCommands.Exceptions;

namespace FluentCommands.Commands
{
    /// <summary>
    /// Declares a method as a <see cref="Command"/> for the <see cref="CommandService"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class CommandAttribute : Attribute
    {
        /// <summary>
        /// Gets the name of this <see cref="Command"/>. Cannot be set outside of the construction of the attribute.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Labels a method to be constructed into a proper <see cref="Command"/> object.
        /// <para><see cref="Command"/> names may only be a maximum of 255 characters, and cannot contain any whitespace characters.</para>
        /// </summary>
        /// <param name="name"></param>
        public CommandAttribute(string name)
        {
            AuxiliaryMethods.CheckCommandNameValidity(name);
            Name = name;
        }
    }
}
