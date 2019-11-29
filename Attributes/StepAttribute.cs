using FluentCommands.Helper;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using FluentCommands.CommandTypes.Steps;

namespace FluentCommands.Attributes
{
    /// <summary>
    /// Labels a method as one of the steps of a <see cref="Command"/>. Return type must be <see cref="Task{T}"/> with <see cref="{TResult}"/> of type <see cref="IStep"/>.
    /// <para>Must also be labeled with the <see cref="CommandAttribute"/> with the name of the parent command that owns this <see cref="Step"/>.</para>
    /// <para>Use the <see cref="Step"/> class to determine what happens if a <see cref="Step"/> succeeds, fails, or needs to undo and return to the last step.</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class StepAttribute : Attribute
    {
        internal int StepNum { get; }

        /// <summary>
        /// Labels a method as one of the steps of a <see cref="Command"/>. Return type must be <see cref="Task{T}"/> with <see cref="{TResult}"/> of type <see cref="IStep"/>.
        /// <para>Must also be labeled with the <see cref="CommandAttribute"/> with the name of the parent command that owns this <see cref="Step"/>.</para>
        /// <para>Use the <see cref="Step"/> class to determine what happens if a <see cref="Step"/> succeeds, fails, or needs to undo and return to the last step.</para>
        /// </summary>
        /// <param name="stepNumber">The number of this step.</param>
        public StepAttribute(int stepNumber) => StepNum = stepNumber;
    }
}
