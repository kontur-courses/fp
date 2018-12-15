using System.Collections.Generic;
using Functional;

namespace Functional
{
    public static class DictionaryExtensions
    {
        public static Result<TValue> Get<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key)
        {
            if (key == null)
                return Functional.Result.Fail<TValue>("Key is null");
            return dictionary.TryGetValue(key, out var value) 
                       ? value.AsResult() 
                       : Functional.Result.Fail<TValue>($"This key {key} doesn't exist in dictionary");
        }
    }
}