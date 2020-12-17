using System;
using System.Collections.Generic;

namespace TagsCloud.Core
{
    public class DisposableDictionary<T1, T2> : IDisposable where T2 : IDisposable
    {
        private readonly Dictionary<T1, T2> dictionary;

        public DisposableDictionary(Dictionary<T1, T2> dictionary)
        {
            this.dictionary = dictionary;
        }

        public T2 this[T1 index] => dictionary[index];

        public void Dispose()
        {
            foreach (var pair in dictionary)
                pair.Value.Dispose();
        }
    }
}