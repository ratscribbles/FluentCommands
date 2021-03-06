﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace FluentCommands.Utility
{
    /// <summary>
    /// A container for reused Regex patterns within the FluentCommands library.
    /// </summary>
    internal struct FluentRegex
    {
        private static readonly TimeSpan _timeOut = new TimeSpan(0, 0, 0, 0, milliseconds: 5000);
        internal static readonly Regex CheckForWhiteSpaces = new Regex(@"(?>[\s]+)", RegexOptions.Compiled, _timeOut);
        internal static readonly Regex CheckNonAlphanumeric = new Regex(@"(?>[^\p{L}0-9_]+)", RegexOptions.Compiled, _timeOut);
        internal static readonly Regex CheckCommand = new Regex(@"([\S]+)(?> (.+))?", RegexOptions.Compiled, _timeOut);
        internal static readonly Regex CheckButtonReference = new Regex(@"^COMMANDBASEBUILDERREFERENCE::([\p{L}0-9_]{1,255})::$", RegexOptions.Compiled, _timeOut);
        internal static readonly Regex CheckButtonLinkedReference = new Regex(@"^COMMANDBASEBUILDERREFERENCE::([\p{L}0-9_]{1,255})::(.{1,32767})::$", RegexOptions.Compiled, _timeOut);
        internal static readonly Regex CheckButtonReferencedCommand = new Regex(@"^BUTTONREFERENCEDCOMMAND::([\p{L}0-9_]{1,255})::$", RegexOptions.Compiled, _timeOut);
    }
}
