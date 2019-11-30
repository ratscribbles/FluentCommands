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
    /// Must also be labeled with the <see cref="CommandAttribute"/> with the name of the parent command that owns this <see cref="Step"/>.
    /// <para>Use the <see cref="Step"/> class to determine what happens if a <see cref="Step"/> succeeds or fails, as well as navigate through your steps.</para>
    /// <para><see cref="Step"/> numbers can be negative, but negative steps can only be accessed with the <see cref="StepExtensions.MoveToStep(Step, int)"/> extension method. For best results, it is recommended to reserve negative steps for special circumstances only.</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class StepAttribute : Attribute
    {
        internal int StepNum { get; }

        /// <summary>
        /// Labels a method as one of the steps of a <see cref="Command"/>. Return type must be <see cref="Task{T}"/> with <see cref="{TResult}"/> of type <see cref="IStep"/>.
        /// Must also be labeled with the <see cref="CommandAttribute"/> with the name of the parent command that owns this <see cref="Step"/>.
        /// <para>Use the <see cref="Step"/> class to determine what happens if a <see cref="Step"/> succeeds or fails, as well as navigate through your steps.</para>
        /// <para><see cref="Step"/> numbers can be negative, but negative steps can only be accessed with the <see cref="StepExtensions.MoveToStep(Step, int)"/> extension method. For best results, it is recommended to reserve negative steps for special circumstances only.</para>
        /// </summary>
        /// <param name="stepNumber">The number of this step.</param>
        public StepAttribute(int stepNumber) => StepNum = stepNumber;
    }
}
