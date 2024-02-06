namespace TagsCloud.Extensions;

public static class StringExtensions
{
    public static bool ContainsAny(this string str, params char[] chars)
    {
        return Array.IndexOf(chars, str) >= 0;
    }
}