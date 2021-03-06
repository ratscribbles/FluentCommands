﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reflection;
using FluentCommands.Exceptions;
using FluentCommands.Steps;

namespace FluentCommands.Commands.Steps
{
    internal class StepContainer
    {
        private readonly IReadOnlyDictionary<int, CommandInvoker<Step>> _invokers;

        internal int TotalCount => _invokers.Count;
        internal (int Positive, int Negative) Count => (_invokers.Count(kvp => kvp.Key > 0), _invokers.Count(kvp => kvp.Key < 0));

        /// <summary>
        /// Index, when positive, accesses normal steps. Negative indexes access debug steps.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        internal CommandInvoker<Step> this[int key] => _invokers[key];

        /// <summary>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="CommandOnBuildingException"></exception>
        /// </summary>
        internal StepContainer(IEnumerable<MethodInfo> methods)
        {
            var dict = new Dictionary<int, CommandInvoker<Step>>();

            string stepsExceptions = "";
            bool firstExceptionIteration = true;
            foreach (var m in methods)
            {
                var commandAttribute = m.GetCustomAttribute<CommandAttribute>() ?? throw new Exception("This exception should never happen. If you encounter it, please contact the creator of this library or put in a bug report. (Command Attribute was null while building StepContainer.)");
                var stepAttribute = m.GetCustomAttribute<StepAttribute>() ?? throw new Exception("This exception should never happen. If you encounter it, please contact the creator of this library or put in a bug report. (Step Attribute was null while building StepContainer.)");
                var num = stepAttribute.StepNum;

                CommandInvoker<Step> invoker;
                try { invoker = new CommandInvoker<Step>(m); }
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
    }
}
