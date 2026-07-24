/*         INFINITY CODE         */
/*   https://infinity-code.com   */

using System;

namespace SilkJson
{
    /// <summary>
    /// Prevents a field from being serialized to JSON.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, Inherited = true)]
    public sealed class JsonNonSerializedAttribute : Attribute
    {
    }
}
