using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace FluentCommands.Exceptions
{
    [Serializable]
    public class InvalidCommandNameException : Exception, ISerializable, IFluentCommandsException
    {
        public string ResourceReferenceProperty { get; set; } = "";
        public string? Description { get; }
        public Exception? Inner { get; }

        public InvalidCommandNameException() { }

        public InvalidCommandNameException(string description) : base(description) { Description = description; }

        public InvalidCommandNameException(string description, Exception inner) : base(description, inner) { Description = description; Inner = inner; }

        protected InvalidCommandNameException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            ResourceReferenceProperty = info.GetString("ResourceReferenceProperty") ?? "";
        }
    }
}
