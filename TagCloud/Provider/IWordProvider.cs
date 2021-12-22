using System.Collections.Generic;

namespace TagCloud.Provider
{
    public interface IWordProvider
    {
        IEnumerable<string> Words { get; }
        void AddWords(IEnumerable<string> words);
    }
}
