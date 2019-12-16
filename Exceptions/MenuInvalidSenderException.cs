using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using FluentCommands.Menus;

namespace FluentCommands.Exceptions
{
    /// <summary>
    /// Thrown when a <see cref="Menu"/> is unable to determine a sender (invalid ChatId or UserId).
    /// </summary>
    [Serializable]
    public class MenuInvalidSenderException : Exception, ISerializable, IFluentCommandsException
    {
        public string ResourceReferenceProperty { get; set; } = "";
        public string? Description { get; }
        public Exception? Inner { get; }

        public MenuInvalidSenderException() { }

        public MenuInvalidSenderException(string description) : base(description) { Description = description; }

        public MenuInvalidSenderException(string description, Exception inner) : base(description, inner) { Description = description; Inner = inner; }

        protected MenuInvalidSenderException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            ResourceReferenceProperty = info.GetString("ResourceReferenceProperty") ?? "";
        }
    }
}
