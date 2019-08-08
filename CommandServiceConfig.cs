using System;
using System.Collections.Generic;
using System.Text;

namespace FluentCommands
{
    // Allow a pass for the "prefix" of commands (of which the default is /)
    // Allow a pass for a match evaluator delegate (of which the default will be, well, yeah)
    // A bool for whether or not to use regex to evaluate.

    public class CommandServiceConfig
    {
        // There can be multiple depending on the module.
        public bool Logging { get; set; }
        public bool UseDefaultRules { get; set; }
        public bool UseDefaultErrorMsg { get; set; }
        public bool UseInternalStateForReplyKeyboards { get; set; }
    }
}
