using FluentCommands.CommandTypes;
using FluentCommands.CommandTypes.Steps;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FluentCommands.Cache
{
    public class StepState
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
                if(IsDefault && c.StepInfo is { }) CommandStepInfo = new CommandStepInfo(c.Name, c.StepInfo.Count, c.StepInfo.TotalCount);
                PreviousStepResult = s.StepResult;
                switch (s.StepAction)
                {
                    case StepAction.Move: Move(s.StepToMove); break;
                    case StepAction.Next: Next(); break;
                    case StepAction.Redo: Redo(); break;
                    case StepAction.Restart: Restart(); break;
                    case StepAction.Undo: Undo(); break;
                    case StepAction.None:
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
    }
}
