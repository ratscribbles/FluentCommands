using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types;

namespace FluentCommands.Cache
{
    //: describe this class and its members
    public sealed class FluentState
    {
        /// <summary>Determines whether this state is in its default state.</summary>
        public bool IsDefault => StepState.IsDefault; // Add to this bool as needed;
        public DateTimeOffset LastUpdated { get; } = DateTimeOffset.Now;
        public FluentKey Key { get; }
        public int BotId => Key.BotId;
        public long ChatId => Key.ChatId;
        public int UserId => Key.UserId;
        public StepState StepState { get; private set; } = new StepState();

        /// <summary>Determines whether or not this user's request is currently being processed.
        /// <para>This property forces an exit of the evaluation of a command if the evaluation is currently in progress.</para></summary>
        internal bool CurrentlyAccessed { get; set; } = false;

        internal FluentState(int botId, long chatId, int userId) => Key = (botId, chatId, userId);
        internal FluentState((int BotId, long ChatId, int UserId) key) => Key = key;

        public bool Equals(FluentState other)
            => other is { }
            && IsDefault.Equals(other.IsDefault)
            && Key.Equals(other.Key)
            && StepState.Equals(other.StepState);

        public override int GetHashCode()
        {
            unchecked
            {
                // Choose large primes to avoid hashing collisions
                const int HashingBase = (int)2166136261;
                const int HashingMultiplier = 16777619;

                int hash = HashingBase;
                hash = (hash * HashingMultiplier) ^ Key.GetHashCode();
                hash = (hash * HashingMultiplier) ^ StepState.GetHashCode();
                return hash;
            }
        }
    }
}
