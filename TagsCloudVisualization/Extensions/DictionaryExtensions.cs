using System;
using System.Collections;
using System.Linq.Expressions;

namespace TagsCloudVisualization.Extensions
{
    public static class DictionaryExtensions
    {
        public static void AddOrUpdate(this IDictionary dictionary, object key, int value,
            Expression<Func<int, int>> func)
        {
            if (dictionary.Contains(key))
                dictionary[key] = func.Compile().Invoke((int)dictionary[key]);
            else
                dictionary[key] = value;
        }
    }
}