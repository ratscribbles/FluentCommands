using FluentCommands.CommandTypes.Steps;
using System;
using System.Collections.Generic;
using System.Text;

namespace FluentCommands.Cache
{
    public class StepState : FluentState
    {
        public override bool IsDefault
        {
            get
            {
                return PreviousStepAction == StepAction.None
                    && PreviousStepResult == StepResult.None
                    && CurrentStepNumber == 0
                    && CommandStepInfo.IsEmpty;
            }
        }
        public CommandStepInfo CommandStepInfo { get; private set; } = CommandStepInfo.Empty;
        public StepAction PreviousStepAction { get; private set; } = StepAction.None;
        public StepResult PreviousStepResult { get; private set; } = StepResult.None;
        public int CurrentStepNumber { get; private set; } = 0;

        internal StepState() { }

        /// <summary>Creates a new <see cref="StepState"/> for the parent <see cref="UserState"/>.
        /// <para>If the <see cref="Command"/> does not have a <see cref="StepContainer"/>, <see cref="IsDefault"/> is true, and it will be assumed the user is not currently in a command with steps.</para></summary>
        internal StepState(Command c)
        {
            if (c.StepInfo is { }) CommandStepInfo = new CommandStepInfo(c.Name, c.StepInfo.Count, c.StepInfo.TotalCount);
        }

        private void ToDefault()
        {
            CommandStepInfo = CommandStepInfo.Empty;
            PreviousStepAction = StepAction.None;
            PreviousStepResult = StepResult.None;
            CurrentStepNumber = 0;
        }

        internal void Next()
        {
            CurrentStepNumber++;
            PreviousStepAction = StepAction.Move;
            if (CurrentStepNumber > CommandStepInfo.CurrentCommandStepCount) ToDefault();
        }
        internal void Undo()
        {
            CurrentStepNumber--;
            PreviousStepAction = StepAction.Undo;
            if (CurrentStepNumber < CommandStepInfo.CurrentCommandStepCount) throw new Exception(); //: shouldn't happen, handle this another way
        }
        internal void Move(int num)
        {
            PreviousStepAction = StepAction.Move;
            CurrentStepNumber = num;
        }
        internal void Redo()
        {
            PreviousStepAction = StepAction.Redo;
        }
        internal void ReturnToStart()
        {
            CurrentStepNumber = 0;
            PreviousStepAction = StepAction.Move;
        }
    }
}
