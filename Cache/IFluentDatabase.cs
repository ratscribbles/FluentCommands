using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace FluentCommands.Cache
{
    public interface IFluentDatabase
    {
        Task AddOrUpdateState(FluentState state);
        Task<FluentState> GetState(long chatId, int userId);
        Task UpdateLastMessage(TelegramBotClient client, long chatId, Message[] messages);
        Task<IReadOnlyCollection<Message>?> GetMessages(TelegramBotClient client, long chatId);
    }
}
