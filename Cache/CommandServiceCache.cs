using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types;

namespace FluentCommands.Cache
{
    internal class CommandServiceCache
    {
        private ConcurrentDictionary<int, ConcurrentDictionary<long, ConcurrentDictionary<int, Message>>> _botLastMessage { get; set; } //: for these two, check if message comes from a private chat (two ppl)
        internal ConcurrentDictionary<int, ConcurrentDictionary<long, ConcurrentDictionary<int, Message>>> _userLastMessage { get; set; }

        //: add one for just rooms in general, maybe? these two might be enough to handle the load, but im not sure.
        //: look into heavy async write solutions with 1 reader. LOTS OF WRITERS, one reader.

        //: try using Channels<T>. provide a boolean for the user to use either unbounded or bounded channels for cacheing (max performance mode true/false (put in the desc that it might use more memory))
    }
}
