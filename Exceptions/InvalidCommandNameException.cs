using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace FluentCommands.Exceptions
{
    [Serializable]
    public class InvalidCommandNameException : Exception, ISerializable
    {
        public string ResourceReferenceProperty { get; set; }

        public InvalidCommandNameException() { }

        public InvalidCommandNameException(string description) : base(description) { }

        public InvalidCommandNameException(string description, Exception inner) : base(description, inner) { }

        protected InvalidCommandNameException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            ResourceReferenceProperty = info.GetString("ResourceReferenceProperty");
        }
    }
}
