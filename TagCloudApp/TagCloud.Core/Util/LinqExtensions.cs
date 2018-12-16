using System;
using System.Collections.Generic;

namespace TagCloud.Core.Util
{
    public static class LinqExtensions
    {
        public static void ApplyForeach<T>(this IEnumerable<T> elements, Action<T> applyingAction)
        {
            foreach (var element in elements)
                applyingAction(element);
        }
    }
}