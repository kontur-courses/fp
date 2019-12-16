using System.Collections.Generic;

namespace TextConfiguration
{
    public interface IWordsProvider
    {
        Result<List<string>> ReadWordsFromFile(string filePath);
    }
}
