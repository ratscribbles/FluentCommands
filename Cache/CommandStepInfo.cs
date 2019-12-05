using System;
using System.Collections.Generic;
using System.Text;

namespace FluentCommands.Cache
{
    public struct CommandStepInfo
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
    }
}
