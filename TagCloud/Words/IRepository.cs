using System.Collections.Generic;

namespace TagCloud.Words
{
    public interface IRepository
    {
        void Load(IEnumerable<string> words);
        IEnumerable<string> Get();
    }
}