using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using FluentCommands.Attributes;
using System.Reflection;
using FluentCommands.Exceptions;

namespace FluentCommands.CommandTypes.Steps
{
    internal class StepContainer
    {
#nullable disable
        private readonly Dictionary<int, CommandInvoker<IStep>> _invokers = new Dictionary<int, CommandInvoker<IStep>> { { 0, null } };
        // private readonly Dictionary<int, Step?> _stepData = new Dictionary<int, Step?> { { 0, null } }; 
        //: no need for this i think. the return is what has the data; you cant save steps preemptively (because the data is supplied on the result of the Task, not anything beforehand)

        internal int TotalCount => _invokers.Count;
        internal (int Positive, int Negative) Count => (_invokers.Count(kvp => kvp.Key > 0), _invokers.Count(kvp => kvp.Key < 0));

        /// <summary>
        /// Index, when positive, accesses normal steps. Negative indexes access debug steps.
        /// <para>Key CANNOT be 0; step 0 is the parent command.</para>
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        internal CommandInvoker<IStep> this[int key]
        {
            get
            {
                if (key == 0) throw new NotSupportedException("Key cannot be 0. Step 0 is the parent command, which must be invoked normally. (If you encounter this error, it was the developer's fault. Please submit a bug report if this exception occurs.)");
                else return _invokers[key];
            }
        }
#nullable enable

        /// <summary>
        /// Note: StepData and DebugStepData index 0 should have no value (null); it'll never be accessed.
        /// </summary>
        internal StepContainer() { }

        /// <summary>
        /// Adds a <see cref="StepAttribute"/> method to the current <see cref="Command"/>'s internal <see cref="StepContainer"/>.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="CommandOnBuildingException"></exception>
        internal void Set_StepInvoker(StepAttribute stepAttribute, MethodInfo method)
        {
            var num = stepAttribute.StepNum;
            if (num == 0) throw new ArgumentException("This exception should never happen. If you encounter it, please contact the creator of this library. (Step number was 0, which is an invalid StepKey to be added to Commands.)");
            if (_invokers.ContainsKey(num)) throw new CommandOnBuildingException("Duplicate Step detected for this Command.");

            _invokers.Add(num, new CommandInvoker<IStep>(method));
        }

    }
}
