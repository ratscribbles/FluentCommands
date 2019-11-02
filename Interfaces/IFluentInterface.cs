using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;

namespace FluentCommands.Interfaces
{
    /// <summary>
    /// Hides the inherited default <see cref="object"/> methods for optimal fluent readability.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IFluentInterface
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        Type GetType();

        [EditorBrowsable(EditorBrowsableState.Never)]
        int GetHashCode();

        [EditorBrowsable(EditorBrowsableState.Never)]
        string? ToString();

        [EditorBrowsable(EditorBrowsableState.Never)]
        bool Equals(object obj);
    }
}
