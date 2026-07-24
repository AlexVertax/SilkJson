using System;

namespace SilkJson
{
    public abstract partial class Json
    {
        /// <summary>
        /// Controls whether failed calls to <see cref="Cast{T}(T, bool?)"/> throw by default.
        /// </summary>
        public static bool ThrowOnInvalidCast = false;
        
        /// <summary>
        /// Converts this JSON primitive to the specified value type.
        /// </summary>
        /// <typeparam name="T">The target value type.</typeparam>
        /// <param name="defaultValue">The value returned when conversion fails and exceptions are disabled.</param>
        /// <param name="throwOnInvalidCast">
        /// Whether to throw on conversion failure, or <see langword="null"/> to use <see cref="ThrowOnInvalidCast"/>.
        /// </param>
        /// <returns>The converted value, or <paramref name="defaultValue"/> when conversion fails and exceptions are disabled.</returns>
        /// <exception cref="InvalidCastException">Conversion failed and exceptions are enabled.</exception>
        public T Cast<T>(T defaultValue = default, bool? throwOnInvalidCast = null) where T : struct
        {
            JsonValue jsonValue = this as JsonValue;
            bool useThrow = throwOnInvalidCast.HasValue ? throwOnInvalidCast.Value : ThrowOnInvalidCast;
            
            if (useThrow)
            {
                if (jsonValue == null) throw new InvalidCastException($"Cannot cast Json to type {typeof(T).Name}");
                if (jsonValue.IsMissed) throw new InvalidCastException($"Cannot cast null Json to type {typeof(T).Name}");
            }
            if (jsonValue == null || jsonValue.IsMissed) return defaultValue;
        
            try
            {
                object value = jsonValue.Value;
                return (T) Convert.ChangeType(value, typeof(T));
            }
            catch (Exception e)
            {
                if (useThrow) throw new InvalidCastException($"Cannot cast Json value '{jsonValue.Value}' of type {jsonValue.Value.GetType().Name} to type {typeof(T).Name}", e);
                return defaultValue;
            }
        }

        #region Type Check Methods

        /// <summary>Determines whether this node contains a Boolean value.</summary>
        /// <returns><see langword="true"/> for a Boolean JSON value; otherwise, <see langword="false"/>.</returns>
        public virtual bool IsBool() => false;

        /// <summary>Determines whether this node contains a floating-point number.</summary>
        /// <returns><see langword="true"/> for a floating-point JSON value; otherwise, <see langword="false"/>.</returns>
        public virtual bool IsDouble() => false;

        /// <summary>Determines whether this node contains an integer number.</summary>
        /// <returns><see langword="true"/> for an integer JSON value; otherwise, <see langword="false"/>.</returns>
        public virtual bool IsLong() => false;

        /// <summary>Determines whether this node contains a string value.</summary>
        /// <returns><see langword="true"/> for a string JSON value; otherwise, <see langword="false"/>.</returns>
        public virtual bool IsString() => false;

        #endregion

        #region Type Cast Methods

        /// <summary>Returns this node as a JSON object.</summary>
        /// <returns>This node as a <see cref="JsonObject"/>, or <see langword="null"/> if it has another type.</returns>
        public JsonObject Obj() => this as JsonObject;

        /// <summary>Returns this node as a JSON array.</summary>
        /// <returns>This node as a <see cref="JsonArray"/>, or <see langword="null"/> if it has another type.</returns>
        public JsonArray Arr() => this as JsonArray;

        /// <summary>Returns this node as a Boolean value.</summary>
        /// <param name="defaultValue">The value returned when conversion fails.</param>
        /// <returns>The converted value, or <paramref name="defaultValue"/>.</returns>
        public bool Bool(bool defaultValue = false) => Cast(defaultValue, false);

        /// <summary>Returns this node as an unsigned 8-bit integer.</summary>
        /// <param name="defaultValue">The value returned when conversion fails.</param>
        /// <returns>The converted value, or <paramref name="defaultValue"/>.</returns>
        public byte Byte(byte defaultValue = 0) => Cast(defaultValue, false);

        /// <summary>Returns this node as a character.</summary>
        /// <param name="defaultValue">The value returned when conversion fails.</param>
        /// <returns>The converted value, or <paramref name="defaultValue"/>.</returns>
        public char Char(char defaultValue = '\0') => Cast(defaultValue, false);

        /// <summary>Returns this node as a signed 16-bit integer.</summary>
        /// <param name="defaultValue">The value returned when conversion fails.</param>
        /// <returns>The converted value, or <paramref name="defaultValue"/>.</returns>
        public short Short(short defaultValue = 0) => Cast(defaultValue, false);

        /// <summary>Returns this node as an unsigned 16-bit integer.</summary>
        /// <param name="defaultValue">The value returned when conversion fails.</param>
        /// <returns>The converted value, or <paramref name="defaultValue"/>.</returns>
        public ushort UShort(ushort defaultValue = 0) => Cast(defaultValue, false);

        /// <summary>Returns this node as a signed 32-bit integer.</summary>
        /// <param name="defaultValue">The value returned when conversion fails.</param>
        /// <returns>The converted value, or <paramref name="defaultValue"/>.</returns>
        public int Int(int defaultValue = 0) => Cast(defaultValue, false);

        /// <summary>Returns this node as an unsigned 32-bit integer.</summary>
        /// <param name="defaultValue">The value returned when conversion fails.</param>
        /// <returns>The converted value, or <paramref name="defaultValue"/>.</returns>
        public uint UInt(uint defaultValue = 0) => Cast(defaultValue, false);

        /// <summary>Returns this node as a signed 64-bit integer.</summary>
        /// <param name="defaultValue">The value returned when conversion fails.</param>
        /// <returns>The converted value, or <paramref name="defaultValue"/>.</returns>
        public long Long(long defaultValue = 0) => Cast(defaultValue, false);

        /// <summary>Returns this node as an unsigned 64-bit integer.</summary>
        /// <param name="defaultValue">The value returned when conversion fails.</param>
        /// <returns>The converted value, or <paramref name="defaultValue"/>.</returns>
        public ulong ULong(ulong defaultValue = 0) => Cast(defaultValue, false);

        /// <summary>Returns this node as a single-precision floating-point number.</summary>
        /// <param name="defaultValue">The value returned when conversion fails.</param>
        /// <returns>The converted value, or <paramref name="defaultValue"/>.</returns>
        public float Float(float defaultValue = 0) => Cast(defaultValue, false);

        /// <summary>Returns this node as a double-precision floating-point number.</summary>
        /// <param name="defaultValue">The value returned when conversion fails.</param>
        /// <returns>The converted value, or <paramref name="defaultValue"/>.</returns>
        public double Double(double defaultValue = 0) => Cast(defaultValue, false);

        /// <summary>Returns this node as a decimal number.</summary>
        /// <param name="defaultValue">The value returned when conversion fails.</param>
        /// <returns>The converted value, or <paramref name="defaultValue"/>.</returns>
        public decimal Decimal(decimal defaultValue = 0) => Cast(defaultValue, false);

        /// <summary>Returns this node as a date and time.</summary>
        /// <param name="defaultValue">The value returned when conversion fails.</param>
        /// <returns>The converted value, or <paramref name="defaultValue"/>.</returns>
        public DateTime DateTime(DateTime defaultValue = default) => Cast(defaultValue, false);

        /// <summary>Returns the textual value of this JSON primitive.</summary>
        /// <param name="defaultValue">The value returned when this node is not a JSON primitive or is null.</param>
        /// <returns>The textual value, or <paramref name="defaultValue"/>.</returns>
        public string String(string defaultValue = null)
        {
            JsonValue jsonValue = this as JsonValue;
            if (jsonValue == null || jsonValue.IsMissed) return defaultValue;
            return jsonValue.ToString();
        }

        #endregion

        #region Implicit and Explicit Operators

        /// <summary>Converts a Boolean value to a JSON value.</summary>
        /// <param name="value">The Boolean value to convert.</param>
        /// <returns>A JSON value representing <paramref name="value"/>.</returns>
        public static implicit operator Json(bool value) => From(value);
        /// <summary>Converts an unsigned 8-bit integer to a JSON value.</summary>
        /// <param name="value">The unsigned 8-bit integer to convert.</param>
        /// <returns>A JSON value representing <paramref name="value"/>.</returns>
        public static implicit operator Json(byte value) => From(value);
        /// <summary>Converts a character to a JSON string value.</summary>
        /// <param name="value">The character to convert.</param>
        /// <returns>A JSON value representing <paramref name="value"/>.</returns>
        public static implicit operator Json(char value) => From(value);
        /// <summary>Converts a signed 16-bit integer to a JSON value.</summary>
        /// <param name="value">The signed 16-bit integer to convert.</param>
        /// <returns>A JSON value representing <paramref name="value"/>.</returns>
        public static implicit operator Json(short value) => From(value);
        /// <summary>Converts an unsigned 16-bit integer to a JSON value.</summary>
        /// <param name="value">The unsigned 16-bit integer to convert.</param>
        /// <returns>A JSON value representing <paramref name="value"/>.</returns>
        public static implicit operator Json(ushort value) => From(value);
        /// <summary>Converts a signed 32-bit integer to a JSON value.</summary>
        /// <param name="value">The signed 32-bit integer to convert.</param>
        /// <returns>A JSON value representing <paramref name="value"/>.</returns>
        public static implicit operator Json(int value) => From(value);
        /// <summary>Converts an unsigned 32-bit integer to a JSON value.</summary>
        /// <param name="value">The unsigned 32-bit integer to convert.</param>
        /// <returns>A JSON value representing <paramref name="value"/>.</returns>
        public static implicit operator Json(uint value) => From(value);
        /// <summary>Converts a signed 64-bit integer to a JSON value.</summary>
        /// <param name="value">The signed 64-bit integer to convert.</param>
        /// <returns>A JSON value representing <paramref name="value"/>.</returns>
        public static implicit operator Json(long value) => From(value);
        /// <summary>Converts an unsigned 64-bit integer to a JSON value.</summary>
        /// <param name="value">The unsigned 64-bit integer to convert.</param>
        /// <returns>A JSON value representing <paramref name="value"/>.</returns>
        public static implicit operator Json(ulong value) => From(value);
        /// <summary>Converts a single-precision floating-point number to a JSON value.</summary>
        /// <param name="value">The single-precision floating-point number to convert.</param>
        /// <returns>A JSON value representing <paramref name="value"/>.</returns>
        public static implicit operator Json(float value) => From(value);
        /// <summary>Converts a double-precision floating-point number to a JSON value.</summary>
        /// <param name="value">The double-precision floating-point number to convert.</param>
        /// <returns>A JSON value representing <paramref name="value"/>.</returns>
        public static implicit operator Json(double value) => From(value);
        /// <summary>Converts a decimal number to a JSON value.</summary>
        /// <param name="value">The decimal number to convert.</param>
        /// <returns>A JSON value representing <paramref name="value"/>.</returns>
        public static implicit operator Json(decimal value) => From(value);
        /// <summary>Converts a string to a JSON value.</summary>
        /// <param name="value">The string to convert.</param>
        /// <returns>A JSON value representing <paramref name="value"/>.</returns>
        public static implicit operator Json(string value) => From(value);
        /// <summary>Converts a date and time to a JSON value.</summary>
        /// <param name="value">The date and time to convert.</param>
        /// <returns>A JSON value representing <paramref name="value"/>.</returns>
        public static implicit operator Json(DateTime value) => From(value);
        /// <summary>Converts a JSON primitive to a Boolean value.</summary>
        /// <param name="json">The JSON primitive to convert.</param>
        /// <returns>The converted Boolean value.</returns>
        public static implicit operator bool (Json json) => json.Cast<bool>();
        /// <summary>Converts a JSON primitive to an unsigned 8-bit integer.</summary>
        /// <param name="json">The JSON primitive to convert.</param>
        /// <returns>The converted unsigned 8-bit integer.</returns>
        public static implicit operator byte (Json json) => json.Cast<byte>();
        /// <summary>Converts a JSON primitive to a character.</summary>
        /// <param name="json">The JSON primitive to convert.</param>
        /// <returns>The converted character.</returns>
        public static implicit operator char (Json json) => json.Cast<char>();
        /// <summary>Converts a JSON primitive to a signed 16-bit integer.</summary>
        /// <param name="json">The JSON primitive to convert.</param>
        /// <returns>The converted signed 16-bit integer.</returns>
        public static implicit operator short (Json json) => json.Cast<short>();
        /// <summary>Converts a JSON primitive to an unsigned 16-bit integer.</summary>
        /// <param name="json">The JSON primitive to convert.</param>
        /// <returns>The converted unsigned 16-bit integer.</returns>
        public static implicit operator ushort (Json json) => json.Cast<ushort>();
        /// <summary>Converts a JSON primitive to a signed 32-bit integer.</summary>
        /// <param name="json">The JSON primitive to convert.</param>
        /// <returns>The converted signed 32-bit integer.</returns>
        public static implicit operator int (Json json) => json.Cast<int>();
        /// <summary>Converts a JSON primitive to an unsigned 32-bit integer.</summary>
        /// <param name="json">The JSON primitive to convert.</param>
        /// <returns>The converted unsigned 32-bit integer.</returns>
        public static implicit operator uint (Json json) => json.Cast<uint>();
        /// <summary>Converts a JSON primitive to a signed 64-bit integer.</summary>
        /// <param name="json">The JSON primitive to convert.</param>
        /// <returns>The converted signed 64-bit integer.</returns>
        public static implicit operator long (Json json) => json.Cast<long>();
        /// <summary>Converts a JSON primitive to an unsigned 64-bit integer.</summary>
        /// <param name="json">The JSON primitive to convert.</param>
        /// <returns>The converted unsigned 64-bit integer.</returns>
        public static implicit operator ulong (Json json) => json.Cast<ulong>();
        /// <summary>Converts a JSON primitive to a single-precision floating-point number.</summary>
        /// <param name="json">The JSON primitive to convert.</param>
        /// <returns>The converted single-precision floating-point number.</returns>
        public static implicit operator float (Json json) => json.Cast<float>();
        /// <summary>Converts a JSON primitive to a double-precision floating-point number.</summary>
        /// <param name="json">The JSON primitive to convert.</param>
        /// <returns>The converted double-precision floating-point number.</returns>
        public static implicit operator double (Json json) => json.Cast<double>();
        /// <summary>Converts a JSON primitive to a decimal number.</summary>
        /// <param name="json">The JSON primitive to convert.</param>
        /// <returns>The converted decimal number.</returns>
        public static implicit operator decimal (Json json) => json.Cast<decimal>();
        /// <summary>Parses the textual JSON value as a date and time.</summary>
        /// <param name="json">The JSON value whose text to parse.</param>
        /// <returns>The parsed date and time.</returns>
        public static implicit operator DateTime (Json json) => System.DateTime.Parse((string) json);
        /// <summary>Returns the string representation of a JSON node.</summary>
        /// <param name="json">The JSON node to represent as text.</param>
        /// <returns>The string representation of <paramref name="json"/>.</returns>
        public static implicit operator string (Json json) => json.ToString();

        #endregion
    }
}
