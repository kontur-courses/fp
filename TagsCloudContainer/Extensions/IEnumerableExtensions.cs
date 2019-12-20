using System;
using System.Collections.Generic;
using System.Linq;

namespace TagsCloudContainer.Extensions
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<(T Element, int Count)> MostCommon<T>(this IEnumerable<T> source, int count)
        {
            return source.GroupBy(e => e)
                .Select(e => (e.Key, e.Count()))
                .OrderByDescending(tuple => tuple.Item2)
                .Take(count);
        }
    }
}