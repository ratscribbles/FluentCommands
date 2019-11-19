using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace FluentCommands.Exceptions
{
    /// <summary>
    /// Thrown when certain conditions are invalid within <see cref="CommandServiceConfig"/> or <see cref="ModuleBuilderConfig"/> objects.
    /// </summary>
    [Serializable]
    public class InvalidConfigSettingsException : Exception, ISerializable, IFluentCommandsException
    {
        public string ResourceReferenceProperty { get; set; } = "";
        public string? Description { get; }
        public Exception? Inner { get; }

        public InvalidConfigSettingsException() { }

        public InvalidConfigSettingsException(string description) : base(description) { Description = description; }

        public InvalidConfigSettingsException(string description, Exception inner) : base(description, inner) { Description = description; Inner = inner; }

        protected InvalidConfigSettingsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            ResourceReferenceProperty = info.GetString("ResourceReferenceProperty") ?? "";
        }
    }
}
