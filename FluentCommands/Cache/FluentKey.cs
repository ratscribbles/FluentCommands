using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace FluentCommands.Cache
{
    /// <summary>
    /// 
    /// </summary>
    //: describe this class l8r lmao
    public struct FluentKey
    {
        public int BotId { get; }
        public long ChatId { get; }
        public int UserId { get; }

        //: desc
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        public FluentKey((int BotId, long ChatId, int UserId) ids) 
        {
            BotId = ids.BotId;
            ChatId = ids.ChatId;
            UserId = ids.UserId;
        }

        public static implicit operator FluentKey((int botId, long chatId, int userId) ids) => new FluentKey(ids);
        public static implicit operator (int botId, long chatId, int userId)(FluentKey key) => (key.BotId, key.ChatId, key.UserId);

        public bool Equals(FluentKey key)
            => key is { }
            && BotId.Equals(key.BotId)
            && ChatId.Equals(key.ChatId)
            && UserId.Equals(key.UserId);

        public override int GetHashCode()
        {
            unchecked
            {
                // Choose large primes to avoid hashing collisions
                const int HashingBase = (int)2166136261;
                const int HashingMultiplier = 16777619;

                int hash = HashingBase;
                hash = (hash * HashingMultiplier) ^ BotId.GetHashCode();
                hash = (hash * HashingMultiplier) ^ ChatId.GetHashCode();
                hash = (hash * HashingMultiplier) ^ UserId.GetHashCode();
                return hash;
            }
        }
    }
}
