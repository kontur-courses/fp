using System;
using System.Collections;
using System.Linq.Expressions;

namespace TagsCloudVisualization.Extensions
{
    public static class DictionaryExtensions
    {
        public static void AddOrUpdate<TKey, TValue>(this IDictionary dictionary, object key, int value,
            Expression<Func<TValue, TValue>> func)
        {
            if (dictionary.Contains(key))
                dictionary[key] = func.Compile().Invoke((TValue)dictionary[key]);
            else
                dictionary[key] = value;
        }
    }
}