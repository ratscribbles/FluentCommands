using FluentCommands.Commands;
using FluentCommands.Commands.Steps;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading.Tasks;

namespace FluentCommands.Cache
{
    public class StepState : IEquatable<StepState>
    {
        public bool IsDefault  
        {
            get
            {
                return PreviousStepAction == StepAction.None
                    && PreviousStepResult == StepResult.None
                    && CurrentStepNumber == 0
                    && PreviousStepNumber == 0
                    && CommandStepInfo.IsEmpty;
            }
        }
        public CommandStepInfo CommandStepInfo { get; private set; } = CommandStepInfo.Empty;
        public StepAction PreviousStepAction { get; private set; } = StepAction.None;
        public StepResult PreviousStepResult { get; private set; } = StepResult.None;
        public int CurrentStepNumber { get; private set; } = 0;
        public int PreviousStepNumber { get; private set; } = 0;


        /// <summary>Creates a new <see cref="StepState"/> for the parent <see cref="FluentState"/>.
        /// <para>If the <see cref="Command"/> does not have a <see cref="StepContainer"/>, <see cref="IsDefault"/> is true, and it will be assumed the user is not currently in a command with steps.</para></summary>
        public StepState() { }

        internal Task Update(ICommand c, IStep s)
        {
            return Task.Run(() =>
            {
                if (IsDefault && c.StepInfo is { }) CommandStepInfo = new CommandStepInfo(c.Name, c.StepInfo.Count, c.StepInfo.TotalCount);
                PreviousStepResult = s.StepResult;

                var tempStepNum = CurrentStepNumber;

                switch (s.StepAction)
                {
                    case StepAction.Move: Move(s.StepToMove); break;
                    case StepAction.Next: Next(); break;
                    case StepAction.Redo: Redo(); break;
                    case StepAction.Restart: Restart(); break;
                    case StepAction.Undo: Undo(); break;
                    case StepAction.None:
                    case StepAction.Stop:
                    default: ToDefault(); break;
                }
            });
        }

        private void ToDefault()
        {
            CommandStepInfo = CommandStepInfo.Empty;
            PreviousStepAction = StepAction.None;
            PreviousStepResult = StepResult.None;
            CurrentStepNumber = 0;
            PreviousStepNumber = 0;
        }

        private void Next()
        {
            PreviousStepAction = StepAction.Next;
            PreviousStepNumber = CurrentStepNumber;
            CurrentStepNumber++;
            if (CurrentStepNumber > CommandStepInfo.CurrentCommandStepCount.Positive) ToDefault();
        }
        private void Undo()
        {
            var tempCurrent = CurrentStepNumber;
            CurrentStepNumber = PreviousStepNumber;
            PreviousStepNumber = tempCurrent;
            PreviousStepAction = StepAction.Undo;
        }
        private void Move(int num)
        {
            PreviousStepAction = StepAction.Move;
            PreviousStepNumber = CurrentStepNumber;
            CurrentStepNumber = num;
        }
        private void Redo()
        {
            PreviousStepAction = StepAction.Redo;
            PreviousStepNumber = CurrentStepNumber;
        }
        private void Restart()
        {
            PreviousStepAction = StepAction.Restart;
            PreviousStepNumber = CurrentStepNumber;
            CurrentStepNumber = 0;
        }

        public bool Equals([AllowNull] StepState other)
        {
            if (other is null) return false;
            if (IsDefault != other.IsDefault) return false;

            return CommandStepInfo.Equals(other.CommandStepInfo)
                && PreviousStepAction == other.PreviousStepAction
                && PreviousStepResult == other.PreviousStepResult
                && PreviousStepNumber == other.PreviousStepNumber
                && CurrentStepNumber == other.CurrentStepNumber;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                // Choose large primes to avoid hashing collisions
                const int HashingBase = (int)2166136261;
                const int HashingMultiplier = 16777619;

                int hash = HashingBase;
                hash = (hash * HashingMultiplier) ^ CommandStepInfo.GetHashCode();
                hash = (hash * HashingMultiplier) ^ PreviousStepAction.GetHashCode();
                hash = (hash * HashingMultiplier) ^ PreviousStepResult.GetHashCode();
                hash = (hash * HashingMultiplier) ^ PreviousStepNumber.GetHashCode();
                hash = (hash * HashingMultiplier) ^ CurrentStepNumber.GetHashCode();
                return hash;
            }
        }
    }
}
