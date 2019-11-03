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
    public class CommandOnBuildingException : Exception, ISerializable
    {
        public string ResourceReferenceProperty { get; set; } = "";

        public CommandOnBuildingException() { }

        public CommandOnBuildingException(string description) : base(description) { }

        public CommandOnBuildingException(string description, Exception inner) : base(description, inner) { }

        protected CommandOnBuildingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            ResourceReferenceProperty = info.GetString("ResourceReferenceProperty") ?? "";
        }
    }
}
