using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Telegram.Bot.Types.ReplyMarkups;

namespace FluentCommands.Exceptions
{
    /// <summary>
    /// Thrown when a <see cref="Menus.MenuItem"/> object has any exceptional issue in building its <see cref="IReplyMarkup"/>.
    /// </summary>
    [Serializable]
    public class MenuReplyMarkupException : Exception, ISerializable, IFluentCommandsException
    {
        public string ResourceReferenceProperty { get; set; } = "";
        public string? Description { get; }
        public Exception? Inner { get; }

        public MenuReplyMarkupException() { }

        public MenuReplyMarkupException(string description) : base(description) { Description = description; }

        public MenuReplyMarkupException(string description, Exception inner) : base(description, inner) { Description = description; Inner = inner; }

        protected MenuReplyMarkupException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            ResourceReferenceProperty = info.GetString("ResourceReferenceProperty") ?? "";
        }
    }
}
