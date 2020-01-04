using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace FluentCommands.Exceptions
{
    [Serializable]
    public class InvalidKeyboardRowException : Exception, ISerializable, IFluentCommandsException
    {
        public string ResourceReferenceProperty { get; set; } = "";
        public string? Description { get; }
        public Exception? Inner { get; }

        public InvalidKeyboardRowException() { }

        public InvalidKeyboardRowException(string description) : base(description) { Description = description; }

        public InvalidKeyboardRowException(string description, Exception inner) : base(description, inner) { Description = description; Inner = inner; }

        protected InvalidKeyboardRowException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            ResourceReferenceProperty = info.GetString("ResourceReferenceProperty") ?? "";
        }
    }
}
