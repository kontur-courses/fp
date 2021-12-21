using ResultOf;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TagsCloudContainer
{
    public static class Enumeration
    {
        public static IEnumerable<T> Repeat<T>(Func<T> get)
        {
            while (true) yield return get();
        }

        public static IEnumerable<T> RepeatUntilNull<T>(Func<T> get)
        {
            return Repeat(get).TakeWhile(res => res != null);
        }

        public static Dictionary<T, int> ConvertToFrequency<T>(IEnumerable<T> col)
        {
            return col.GroupBy(word => word)
                .ToDictionary(w => w.Key, w => w.Count());
        }
    }
}
