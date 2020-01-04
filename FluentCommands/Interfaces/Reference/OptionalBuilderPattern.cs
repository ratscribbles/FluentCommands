using FluentCommands.Interfaces.MenuBuilders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FluentCommands.Interfaces.Reference
{
    internal class FluentBuilder_With_Steps_Default_Implementation_Example
    {
        /* This class is a reference that serves as a blueprint for how to implement
         * Step-by-Step fluent builder patterns that have each step as "optional".
         * 
         * It reduces the amount of code-reuse dramatically by utilizing C# 8's 
         * "Default Interface Methods" feature to effectively eliminate the need to
         * provide a concrete implementation of each step of the interface, while also
         * adding the bonus of complete modularity of behaviors away from the steps themselves.
         * 
         * Simply write "Implementation Interfaces", a unifying interface for each series of steps,
         * and implement the properties automatically with VS.
         * 
         * In order to retrieve these properties, only one set of concretions needs to be implemented.
         * 
         * This pattern is used -extensively- by this library, where many different ways to send
         * Message objects have optional fields, but different types can have different optional behaviors.
         * 
         * -- ash f
         */

        private class Explanation
        {
            internal interface IUnifyingBuilderImplementationInterface
            {
                // Requires concretion later.
                internal string Retrieve_Required();
            }

            public interface IBuildAction<TNextBuilderImplementation> where TNextBuilderImplementation : IUnifyingBuilderImplementationInterface
            {
                internal string In_Required { get; set; }
                TNextBuilderImplementation DoSomething(string input)
                {
                    /* Set a property that's required in the IUnifying Interface */
                    In_Required = input;
                    return (TNextBuilderImplementation)this;
                }
            }

            // No concrete implementations needed.
            public interface IBuilderImplementationStep1 : IUnifyingBuilderImplementationInterface, IBuildAction<IBuilderImplementationStep2>,
                /* Implement the final "Implementation Interface" if it has a finalizing method. */IBuilderImplementationStep2
            { }

            public interface IBuilderImplementationStep2 : IUnifyingBuilderImplementationInterface
            {
                // Add any additional methods here. Great for ending fluent building with DIMs that return void:
                void Done() { /* Set some property. */ } // No concretion needed.
            }

            internal class ImplementingClass : IBuilderImplementationStep1
            {
                // Add required properties as needed; use VS to generate all explicit implementations automatically.
                string IBuildAction<IBuilderImplementationStep2>.In_Required { get; set; }

                // Add concrete implementation of the unifying interface.
                string IUnifyingBuilderImplementationInterface.Retrieve_Required()
                    => (this as IBuildAction<IBuilderImplementationStep2>).In_Required;

                // Static method to begin the building process here. (If desired.)
                internal static IBuilderImplementationStep1 StartBuilding() => new ImplementingClass();
            }
            private class Example
            {
                void M()
                {
                    ImplementingClass.StartBuilding().DoSomething("").Done();
                    ImplementingClass.StartBuilding().Done();

                    IBuilderImplementationStep1 obj = new ImplementingClass();
                    obj.DoSomething("").Done();
                    obj.Done();
                }
            }
        }
    }
}
