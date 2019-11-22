using System;
using System.Collections.Generic;
using System.Text;

namespace FluentCommands
{
    public class Step
    {
        internal enum StepResult { Failure, Success, Undo }

        internal StepResult Result { get; }
        internal Action? SuccessAction { get; }
        internal Action? FailureAction { get; }
        internal Action? UndoAction { get; }

        private Step(StepResult result) => Result = result;
        private Step(Action action, StepResult result)
        {
            switch (result)
            {
                case StepResult.Failure:
                    FailureAction = action; Result = result;
                    break;
                case StepResult.Success:
                    SuccessAction = action; Result = result;
                    break;
                case StepResult.Undo:
                    UndoAction = action; Result = result;
                    break;
            }

        }

        public static Step Success() => new Step(StepResult.Success);
        public static Step Failure() => new Step(StepResult.Failure);
        public static Step Undo() => new Step(StepResult.Undo);

        public static Step Success(Action successAction) => new Step(successAction, StepResult.Success);
        public static Step Failure(Action failureAction) => new Step(failureAction, StepResult.Failure);
        public static Step Undo(Action undoAction) => new Step(undoAction, StepResult.Undo);
    }
}