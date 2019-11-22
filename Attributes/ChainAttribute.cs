using FluentCommands.Helper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace FluentCommands.Attributes
{
    /// <summary>
    /// Declares a method as a <see cref="CommandTypes.ChainCommand"/> for the <see cref="CommandService"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class ChainAttribute : Attribute
    {
        /// <summary>Gets the name of this <see cref="Command"/>. Cannot be set outside of the construction of the attribute.</summary>
        internal string NameOrPattern { get; }
        /// <summary>Gets the name of the <see cref="Command"/> this command is linked to.</summary>
        internal string ParentName { get; }
        internal RegexOptions RegexOptions { get; }

        /// <summary>
        /// Labels a method to be linked to another <see cref="Command"/> or other <see cref="ChainAttribute"/> method.
        /// <para>Methods marked with a <see cref="ChainAttribute"/> can only occur after their parent <see cref="Command"/> or <see cref="ChainAttribute"/> methods have been invoked.</para>
        /// <para>Chains may contain spaces or special characters.</para>
        /// </summary>
        /// <param name="nameOrPattern"></param>
        /// <param name="parentCommandName"></param>
        public ChainAttribute(string nameOrPattern, string parentCommandOrChain)
        {
            NameOrPattern = nameOrPattern;
            ParentName = parentCommandOrChain;
        }

        public ChainAttribute(string nameOrPattern, string parentCommandOrChain, RegexOptions regexOptions)
        {
            NameOrPattern = nameOrPattern;
            ParentName = parentCommandOrChain;
            RegexOptions = regexOptions;
        }
    }
}
