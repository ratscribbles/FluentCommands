using System;
using System.Collections.Generic;
using System.Text;

namespace FluentCommands
{
    //: Allow a pass for the "prefix" of commands (of which the default is /)
    //? should be the module config lol.


    //? unnecessary
    //: Allow a pass for a match evaluator delegate (of which the default will be, well, yeah)
    //: A bool for whether or not to use regex to evaluate.
    //? unnecessary

    public class CommandServiceConfig
    {
        // There can be multiple depending on the module.
        public bool Logging { get; }
        public bool UseDefaultRules { get; }
        public bool UseDefaultErrorMsg { get; }
        public bool UseInternalStateForReplyKeyboards { get; }

        public CommandServiceConfig() { }
    }
}
