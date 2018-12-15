using System.Collections.Generic;

namespace TagCloud.Words
{
    public interface IExcludingWordRepository
    {
        void Load(IEnumerable<string> words);
        bool Contains(string word);
    }
}