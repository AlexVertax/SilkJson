using System;
using System.Collections.Generic;
using System.Linq;

namespace SilkJson
{
    /// <summary>
    /// Provides iteration helpers for JSON sequences and grouped JSON sequences.
    /// </summary>
    public static class JsonExtensions
    {
        /// <summary>
        /// Performs an action for every JSON node in a sequence.
        /// </summary>
        /// <param name="items">The JSON nodes to enumerate.</param>
        /// <param name="action">The action to perform for each node.</param>
        public static void ForEach(this IEnumerable<Json> items, Action<Json> action)
        {
            foreach (Json item in items) action(item);
        }
    
        /// <summary>
        /// Performs an action for every group in a grouped JSON sequence.
        /// </summary>
        /// <typeparam name="TKey">The type of the group key.</typeparam>
        /// <param name="groups">The groups to enumerate.</param>
        /// <param name="action">The action that receives each group key and its JSON nodes.</param>
        public static void ForEach<TKey>(this IEnumerable<IGrouping<TKey, Json>> groups, Action<TKey, IEnumerable<Json>> action)
        {
            foreach (IGrouping<TKey, Json> group in groups) action(group.Key, group);
        }
    }
}
