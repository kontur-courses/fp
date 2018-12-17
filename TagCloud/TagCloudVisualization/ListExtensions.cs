using System;
using System.Collections.Generic;
using System.Linq;
using Functional;

namespace TagCloudVisualization
{
    public static class ListExtensions
    {
        public static Result<T> GetFirst<T>(this List<T> list, Func<T, bool> predicate) where T : class
        {
            var result = list.FirstOrDefault(predicate);
            return result ?? Result.Fail<T>("There is no element matching given predicate");
        }
    }
}
