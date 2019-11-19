using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace FluentCommands.Exceptions
{
    /// <summary>
    /// Thrown when the <see cref="CommandService"/> encounters an unrecoverable state after initializing.
    /// </summary>
    [Serializable]
    public class CommandOnBuildingException : Exception, ISerializable, IFluentCommandsException
    {
        public string ResourceReferenceProperty { get; set; } = "";
        public string? Description { get; }
        public Exception? Inner { get; }

        public CommandOnBuildingException() { }

        public CommandOnBuildingException(string description) : base(description) { Description = description; }

        public CommandOnBuildingException(string description, Exception inner) : base(description, inner) { Description = description; Inner = inner; }

        protected CommandOnBuildingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            ResourceReferenceProperty = info.GetString("ResourceReferenceProperty") ?? "";
        }
    }
}
