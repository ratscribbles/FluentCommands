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
        private readonly IReadOnlyDictionary<int, CommandInvoker<IStep>> _invokers = new Dictionary<int, CommandInvoker<IStep>> { { 0, null } };
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

        /// <summary>
        /// Note: StepData and DebugStepData index 0 should have no value (null); it'll never be accessed.
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="CommandOnBuildingException"></exception>
        /// </summary>
        internal StepContainer(IEnumerable<MethodInfo> methods)
        {
            var dict = new Dictionary<int, CommandInvoker<IStep>> { { 0, null } };

            string stepsExceptions = "";
            bool firstExceptionIteration = true;
            foreach (var m in methods)
            {
                var commandAttribute = m.GetCustomAttribute<CommandAttribute>() ?? throw new Exception("This exception should never happen. If you encounter it, please contact the creator of this library or put in a bug report. (Command Attribute was null while building StepContainer.)");
                var stepAttribute = m.GetCustomAttribute<StepAttribute>() ?? throw new Exception("This exception should never happen. If you encounter it, please contact the creator of this library or put in a bug report. (Step Attribute was null while building StepContainer.)");
                var num = stepAttribute.StepNum;

                if (num == 0) continue; // does not count as an iteration of this loop; it's excluded for the purposes of the iteration check
                if (_invokers.ContainsKey(num)) throw new CommandOnBuildingException("Duplicate Step detected for this Command.");

                CommandInvoker<IStep> invoker;
                try { invoker = new CommandInvoker<IStep>(m); }
                catch(ArgumentException) 
                {
                    if (firstExceptionIteration) stepsExceptions += $"Step {num}";
                    else stepsExceptions += $", Step {num}";
                    firstExceptionIteration = false;
                    continue;
                }

                dict.Add(num, invoker);
            }

            if (stepsExceptions != "") throw new ArgumentException($"{stepsExceptions} had invalid method signature(s).");

            _invokers = dict;
        }
#nullable enable    

    }
}
