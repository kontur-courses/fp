using System.Collections.Generic;
using ResultOf;

namespace TagCloud.Factories
{
    public interface IFileReader
    {
        Result<IEnumerable<string>> ReadWordsFromFile(string pathToFile);
    }
}
