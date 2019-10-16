using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace FluentCommands
{
    /// <summary>
    /// A container for reused Regex patterns within the FluentCommands library.
    /// </summary>
    internal struct FluentRegex
    {
        private static TimeSpan _timeOut = new TimeSpan(5000);

        internal static Regex CheckForWhiteSpaces = new Regex(@"(?>[\s]+)", RegexOptions.Compiled, _timeOut);
        internal static Regex CheckCommand = new Regex(@"([\S]+)(?> (.+))?", RegexOptions.Compiled, _timeOut);
    }
}
