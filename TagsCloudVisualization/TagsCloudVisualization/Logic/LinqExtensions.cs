using System;
using System.Collections.Generic;

namespace TagsCloudVisualization.Logic
{
    public static class LinqExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach(T item in source)
                action(item);
        }
    }
}