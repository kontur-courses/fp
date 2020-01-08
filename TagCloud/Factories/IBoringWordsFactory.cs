using System.Collections.Generic;
using ResultOf;

namespace TagCloud.Factories
{
    public interface IBoringWordsFactory
    {
        Result<HashSet<string>> GetFromFile(string fileName);
    }
}