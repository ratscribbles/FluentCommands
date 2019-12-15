using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace FluentCommands.Cache
{
    public struct CommandStepInfo : IEquatable<CommandStepInfo>
    {
        internal static CommandStepInfo Empty = new CommandStepInfo("", (0,0), 0);

        public bool IsEmpty { get; }
        public (int Positive, int Negative) CurrentCommandStepCount { get; }
        public int CurrentCommandTotalStepCount { get; }
        public string CurrentCommandName { get; }

        internal CommandStepInfo(string commandName, (int Positive, int Negative) stepCount, int totalStepCount)
        {
            CurrentCommandName = commandName;
            CurrentCommandStepCount = stepCount;
            CurrentCommandTotalStepCount = totalStepCount;
            IsEmpty = commandName == "" && stepCount == (0,0) && totalStepCount == 0;
        }

        public bool Equals(CommandStepInfo other)
             => CurrentCommandStepCount.Positive == other.CurrentCommandStepCount.Positive
                && CurrentCommandStepCount.Negative == other.CurrentCommandStepCount.Negative
                && CurrentCommandTotalStepCount == other.CurrentCommandTotalStepCount
                && CurrentCommandName == other.CurrentCommandName;

        public override int GetHashCode()
        {
            unchecked
            {
                // Choose large primes to avoid hashing collisions
                const int HashingBase = (int)2166136261;
                const int HashingMultiplier = 16777619;

                int hash = HashingBase;
                hash = (hash * HashingMultiplier) ^ CurrentCommandStepCount.Positive.GetHashCode();
                hash = (hash * HashingMultiplier) ^ CurrentCommandStepCount.Negative.GetHashCode();
                hash = (hash * HashingMultiplier) ^ CurrentCommandTotalStepCount.GetHashCode();
                hash = (hash * HashingMultiplier) ^ (CurrentCommandName is { } ? CurrentCommandName.GetHashCode() : 0);
                return hash;
            }
        }
    }
}
