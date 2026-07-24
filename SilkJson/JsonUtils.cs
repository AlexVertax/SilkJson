using System;
using System.Collections.Generic;
using System.Linq;

namespace SilkJson
{
    /// <summary>
    /// Provides helper methods used to navigate and format JSON data.
    /// </summary>
    public static class JsonUtils
    {
        /// <summary>
        /// Creates an indentation string for the specified depth.
        /// </summary>
        /// <param name="depth">The number of indentation units to append.</param>
        /// <param name="indent">The indentation unit.</param>
        /// <returns>The indentation unit repeated <paramref name="depth"/> times.</returns>
        public static string GetIndent(int depth, string indent)
        {
            return string.Concat(Enumerable.Repeat(indent, depth));
        }

        /// <summary>
        /// Normalizes a path by expanding slash-delimited string keys and deep-search markers.
        /// </summary>
        /// <param name="keys">The property names and array indexes that form the path.</param>
        /// <returns>The normalized path, or an empty array when <paramref name="keys"/> is <see langword="null"/>.</returns>
        public static StringOrIntValue[] PrepareKeys(StringOrIntValue[] keys)
        {
            if (keys == null) return Array.Empty<StringOrIntValue>();
            
            List<StringOrIntValue> newKeys = new List<StringOrIntValue>(keys.Length);
            for (int i = 0; i < keys.Length; i++)
            {
                StringOrIntValue key = keys[i];
                if (key.IsString)
                {
                    string sKey = key;
                    if (sKey.Contains('/'))
                    {
                        string[] parts = sKey.Replace("//", "/|").Split('/', StringSplitOptions.RemoveEmptyEntries);
                        newKeys.AddRange(parts.Select(p => (StringOrIntValue)p));
                    }
                    else newKeys.Add(key);
                }
                else newKeys.Add(key);
            }
            return newKeys.ToArray();
        }
    }
}
