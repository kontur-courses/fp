using System.Collections.Generic;

namespace Result
{
    public static class DictionaryExtensions
    {
        public static Result<TValue> Get<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key)
        {
            if (key == null)
                return Result.Fail<TValue>("Key is null");
            return dictionary.TryGetValue(key, out var value)
                       ? value.AsResult()
                       : Result.Fail<TValue>($"This key {key} doesn't exist in dictionary");
        }
    }
}
