using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace FluentCommands.Exceptions
{
    /// <summary>
    /// Thrown when a <see cref="Command"/> of the same name already exists within <see cref="CommandService"/>.
    /// </summary>
    [Serializable]
    public class DuplicateCommandException : Exception, ISerializable
    {
        public string ResourceReferenceProperty { get; set; } = "";

        public DuplicateCommandException() { }

        public DuplicateCommandException(string description) : base(description) { }

        public DuplicateCommandException(string description, Exception inner) : base(description, inner) { }

        protected DuplicateCommandException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            ResourceReferenceProperty = info.GetString("ResourceReferenceProperty") ?? "";
        }
    }
}
