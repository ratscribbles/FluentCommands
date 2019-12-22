using System;
using System.Collections.Generic;
using System.Text;

namespace FluentCommands.Utility
{
    internal class ServiceContainer<T> where T : class
    {
        internal Type ModuleType { get; }
        internal Type ServiceType { get; } = typeof(T);
        internal Type? ImplementationType { get; }
        internal T? ImplementationInstance { get; }

        internal ServiceContainer(Type moduleType, Type implementationType) { ModuleType = moduleType; ImplementationType = implementationType; }
        internal ServiceContainer(Type moduleType, T instance) { ModuleType = moduleType; ImplementationInstance = instance; }
    }
}
