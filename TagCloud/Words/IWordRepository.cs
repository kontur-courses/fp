using System.Collections.Generic;

namespace TagCloud.Words
{
    public interface IWordRepository
    {
        void Load(IEnumerable<string> words);
        IEnumerable<string> Get();
    }
}