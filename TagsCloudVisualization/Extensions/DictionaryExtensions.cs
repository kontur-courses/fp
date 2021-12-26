using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace TagsCloudVisualization.Extensions
{
    public static class DictionaryExtensions
    {
        public static void AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value,
            Expression<Func<TValue, TValue>> func)
        {
            if (dictionary.ContainsKey(key))
                dictionary[key] = func.Compile().Invoke(dictionary[key]);
            else
                dictionary[key] = value;
        }
    }
}