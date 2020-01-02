using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FluentCommands.Interfaces
{
    internal interface IEvaluationStrategy
    {
        Task Evaluate();
    }
}
