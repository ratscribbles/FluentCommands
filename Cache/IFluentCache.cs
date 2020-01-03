using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace FluentCommands.Cache
{
    public interface IFluentCache
    {
        Task AddOrUpdateState(FluentState state);
        Task<FluentState> GetState(int botId, long chatId, int userId);
        Task UpdateLastMessage(int botId, long chatId, int userId, IEnumerable<Message?> messages);
        Task<IEnumerable<Message?>> GetMessages(int botId, long chatId, int userId);
    }
}
