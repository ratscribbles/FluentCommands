using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace FluentCommands.Cache
{
    internal class CommandServiceCache : IFluentDatabase
    {
        /// <summary>Last message(s) sent by the bot.<para>int is botId, long is chatId.</para></summary>
        //! if the user is the bot's id, that state means any user can interact with the state
        //: maybe? to the above; not sure if i should have 0 mean anyone can interact with the bot
        private readonly ConcurrentDictionary<FluentKey, FluentState> _stateCache = new ConcurrentDictionary<FluentKey, FluentState>();
        private readonly ConcurrentDictionary<FluentKey, Message[]> _messageCache = new ConcurrentDictionary<FluentKey, Message[]>();

        internal CommandServiceCache() { }

        public Task AddOrUpdateState(FluentState state)
            => Task.Run(() => _stateCache[state.Key] = state);
        public Task<FluentState> GetState(int botId, long chatId, int userId)
            => Task.Run(() => { var key = (botId, chatId, userId); return _stateCache.GetOrAdd(key, (key) => new FluentState(key)); });

        //public Task<FluentState> GetState(long chatId, int userId)
        //    => Task.Run(() => { _stateCache.TryGetValue((chatId, userId), out var state); return state ?? new FluentState(chatId, userId); });
        //: Just in case the above implementation breaks for whatever reason...

        public Task UpdateLastMessage(int botId, long chatId, int userId, Message[] messages)
            => Task.Run(() => _messageCache[(botId, chatId, userId)] = messages);
        public Task<IReadOnlyCollection<Message>?> GetMessages(int botId, long chatId, int userId)
            => Task.Run(() => { _messageCache.TryGetValue((botId, chatId, userId), out var messages); return messages as IReadOnlyCollection<Message>; });

        //: Consider channels<T> at a later date. This is the default implementation.
    }
}
