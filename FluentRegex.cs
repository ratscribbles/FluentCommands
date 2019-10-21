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
        internal static Regex CheckNonAlphanumeric = new Regex(@"(?>[^\p{L}_]+)", RegexOptions.Compiled, _timeOut);
        internal static Regex CheckCommand = new Regex(@"([\S]+)(?> (.+))?", RegexOptions.Compiled, _timeOut);
        internal static Regex CheckButtonReference = new Regex(@"^COMMANDBASEBUILDERREFERENCE::([\p{L}_]{1,255})::$", RegexOptions.Compiled, _timeOut);
        internal static Regex CheckButtonLinkedReference = new Regex(@"^COMMANDBASEBUILDERREFERENCE::([\p{L}_]{1,255})::(.{1,32767})::$", RegexOptions.Compiled, _timeOut);
        internal static Regex CheckButtonReferencedCommand = new Regex(@"^BUTTONREFERENCEDCOMMAND::([\p{L}_]{1,255})::$", RegexOptions.Compiled, _timeOut);
    }
}
