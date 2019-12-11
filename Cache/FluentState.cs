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

        public int UserId { get; }
        public User User { get; }
        public long ChatId { get; }
        public Chat Chat { get; }
        public bool IsMe { get; }
        public StepState StepState { get; private set; } = new StepState();

        internal FluentState(Chat c, User u, bool isMe = false)
        {
            User = u;
            Chat = c;
            UserId = u.Id;
            ChatId = c.Id;
            IsMe = isMe;
        }
    }
}
