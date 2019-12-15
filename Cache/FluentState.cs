using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types;

namespace FluentCommands.Cache
{
    public sealed class FluentState
    {
        /// <summary>Determines whether this state is in its default state.</summary>
        public bool IsDefault => StepState.IsDefault; // Add to this bool as needed;
        //internal static TState Default<TState>() where TState : FluentState, new() => new TState();
        public long ChatId { get; }
        public int UserId { get; }
        public bool IsMe { get; }
        public StepState StepState { get; private set; } = new StepState();

        /// <summary>Determines whether or not this user's request is currently being processed.
        /// <para>This property forces an exit of the evaluation of a command if the evaluation is currently in progress.</para></summary>
        internal bool CurrentlyAccessed { get; set; } = false;

        internal FluentState(long chatId, int userId, bool isMe = false)
        {
            UserId = userId;
            ChatId = chatId;
            IsMe = isMe;
        }
    }
}
