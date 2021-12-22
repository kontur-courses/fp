using System.Collections.Generic;

namespace TagCloud.Extensions;

public static class EnumerableExtensions
{
    public static Dictionary<T, int> GetCountByItems<T>(this IEnumerable<T> items)
    {
        var wordToCount = new Dictionary<T, int>();
        foreach (var item in items)
        {
            wordToCount.TryGetValue(item, out var currentCount);
            wordToCount[item] = currentCount + 1;
        }

        return wordToCount;
    }
}