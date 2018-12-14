using System.Collections.Generic;

namespace TagsCloudContainer.WordsReaders
{
    public interface IWordsReader
    {
        Result<IEnumerable<string>> GetWords(string filename);
    }
}