using System;
using System.Collections.Generic;
using System.Text;

namespace FluentCommands.CommandTypes.Steps
{
    internal class StepContainer
    {
        private readonly Step[] _stepData;
        private readonly Step[] _negativeStepData;
        
        /// <summary>
        /// Index, when positive, accesses normal steps. Negative indexes access debug steps.
        /// <para>Key CANNOT be 0; step 0 is the parent command.</para>
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        internal Step this[int key]
        {
            get
            {
                if (key > 0) return _stepData[key];
                else if (key < 0) return _negativeStepData[-key];
                else if (key == 0) throw new NotSupportedException("Key cannot be 0. Step 0 is the parent command, which must be invoked normally. (If you encounter this error, it was the developer's fault. Please submit a bug report if this exception occurs.)");
                else throw new IndexOutOfRangeException("Index was out of range when attempting to access this StepContainer's internal StepData.");
            }
        }

        /// <summary>
        /// Note: StepData and DebugStepData index 0 should have no value (null); it'll never be accessed.
        /// </summary>
        /// <param name="stepData"></param>
        /// <param name="negativeStepData"></param>
        internal StepContainer(Step[] stepData, Step[] negativeStepData)
        {
            _stepData = stepData;
            _negativeStepData = negativeStepData;
        }
    }
}
