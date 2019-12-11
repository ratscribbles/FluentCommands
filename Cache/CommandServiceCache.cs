using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace FluentCommands.Cache
{
    internal class CommandServiceCache : IFluentDatabase
    {
        /// <summary>Last message(s) sent by the bot.<para>int is botId, long is chatId.</para></summary>
        //! if the user is the bot's id, that state means any user can interact with the state
        private readonly ConcurrentDictionary<(long ChatId, int UserId), FluentState> _cache = new ConcurrentDictionary<(long ChatId, int UserId), FluentState>();
        
        public Task AddOrUpdateState(FluentState state)
            => Task.Run(() => _cache[(state.ChatId, state.UserId)] = state);

        public Task<FluentState?> GetState(long chatId, int userId)
            => Task.Run(() => { _cache.TryGetValue((chatId, userId), out var state); return state; });


        //: Channel implementation (probably unneeded, but kept commented just in case)

        //private readonly Channel<FluentState> _channel = Channel.CreateBounded<FluentState>(50000);
        //
        //public async Task AddOrUpdateState(FluentState state)
        //{
        //    await _channel.Writer.WriteAsync(state);
        //    await On_Update();
        //}
        //private async Task On_Update()
        //{
        //    var state = await _channel.Reader.ReadAsync();
        //    _cache[(state.ChatId, state.UserId)] = state;
        //}

        //: add one for just rooms in general, maybe? these two might be enough to handle the load, but im not sure.
        //: look into heavy async write solutions with 1 reader. LOTS OF WRITERS, one reader.

        //: try using Channels<T>. provide a boolean for the user to use either unbounded or bounded channels for cacheing (max performance mode true/false (put in the desc that it might use more memory))
    }
}
