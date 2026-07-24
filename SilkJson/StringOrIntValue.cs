using System;

namespace SilkJson
{
    /// <summary>
    /// Represents either a string property name or an integer array index in a JSON path.
    /// </summary>
    public class StringOrIntValue
    {
        private readonly object _value;

        /// <summary>Gets whether this value contains an integer array index.</summary>
        public bool IsInt => !IsString;

        /// <summary>Gets whether this value contains a string property name.</summary>
        public bool IsString { get; }

        private StringOrIntValue(object value)
        {
            _value = value;
            IsString = value is string;
        }

        /// <summary>Converts an integer array index to a path value.</summary>
        /// <param name="value">The array index to wrap.</param>
        /// <returns>A path value containing <paramref name="value"/> as an array index.</returns>
        public static implicit operator StringOrIntValue(int value) => new(value);

        /// <summary>Converts a string property name to a path value.</summary>
        /// <param name="value">The property name to wrap.</param>
        /// <returns>A path value containing <paramref name="value"/> as a property name.</returns>
        public static implicit operator StringOrIntValue(string value) => new(value);

        /// <summary>Returns the integer array index stored in a path value.</summary>
        /// <param name="value">The path value to unwrap.</param>
        /// <returns>The stored integer array index.</returns>
        public static implicit operator int(StringOrIntValue value) => !value.IsString ? (int)value._value : throw new InvalidCastException("Value is not an int");

        /// <summary>Returns the string property name stored in a path value.</summary>
        /// <param name="value">The path value to unwrap.</param>
        /// <returns>The stored string property name.</returns>
        public static implicit operator string(StringOrIntValue value) => value.IsString ? (string)value._value : throw new InvalidCastException("Value is not a string");
    }
}
