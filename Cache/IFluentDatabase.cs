﻿using System;
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
        Task<FluentState> GetState(int botId, long chatId, int userId);
        Task UpdateLastMessage(int botId, long chatId, int userId, Message[] messages);
        Task<IReadOnlyCollection<Message>?> GetMessages(int botId, long chatId, int userId);
    }
}
