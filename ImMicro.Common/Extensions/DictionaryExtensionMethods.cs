using System.Collections.Generic;

namespace ImMicro.Common.Extensions
{
    public static class DictionaryExtensionMethods
    {
        /// <summary>
        /// Merge Dictionary
        /// </summary>
        /// <param name="source">Source Dictionary</param>
        /// <param name="merge">Dictionary for merge</param>
        /// <typeparam name="TKey">TKey</typeparam>
        /// <typeparam name="TValue">TValue</typeparam>
        public static void Merge<TKey, TValue>(this Dictionary<TKey, TValue> source, Dictionary<TKey, TValue> merge)
        {
            foreach (var item in merge)
            {
                source[item.Key] = item.Value;
            }
        }
    }
}