using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FluentCommands.Cache;
using Telegram.Bot.Types;

namespace FluentCommands.Tests.Integration
{
    class TestCache : IFluentCache
    {
        public Task AddOrUpdateState(FluentState state)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyCollection<Message>?> GetMessages(int botId, long chatId, int userId)
        {
            throw new NotImplementedException();
        }

        public Task<FluentState> GetState(int botId, long chatId, int userId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateLastMessage(int botId, long chatId, int userId, Message[] messages)
        {
            throw new NotImplementedException();
        }
    }
}
