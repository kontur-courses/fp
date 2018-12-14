using System.Collections.Generic;

namespace TagCloud.Words
{
    public interface IExcludingRepository
    {
        void Load(IEnumerable<string> words);
        bool Contains(string word);
    }
}