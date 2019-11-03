using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace FluentCommands.Exceptions
{
    [Serializable]
    public class InvalidKeyboardRowException : Exception, ISerializable
    {
        public string ResourceReferenceProperty { get; set; } = "";

        public InvalidKeyboardRowException() { }

        public InvalidKeyboardRowException(string description) : base(description) { }

        public InvalidKeyboardRowException(string description, Exception inner) : base(description, inner) { }

        protected InvalidKeyboardRowException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            ResourceReferenceProperty = info.GetString("ResourceReferenceProperty") ?? "";
        }
    }
}
