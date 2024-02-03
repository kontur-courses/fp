namespace TagCloud.Extensions;

public static class EnumerableExtension
{
    public static IEnumerable<T> Repeat<T>(Func<T> get)
    {
        while (true) yield return get();
    }
    
    public static IEnumerable<T> RepeatUntilNull<T>(this Func<T> get)
    {
        return Repeat(get).TakeWhile(x => x != null);
    }
    
    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
        foreach (var element in source) action(element);
    }
    
    public static Dictionary<T, int> CountValues<T>(this IEnumerable<T> source) where T : notnull
    {
        var counter = new Dictionary<T, int>();
        foreach (var element in source)
        {
            counter.TryAdd(element, 0);
            counter[element]++;
        }
        return counter;
    }
}