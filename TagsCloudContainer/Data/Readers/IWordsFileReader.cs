using System.Collections.Generic;
using TagsCloudContainer.Functional;

namespace TagsCloudContainer.Data.Readers
{
    public interface IWordsFileReader
    {
        Result<IEnumerable<string>> ReadAllWords(string path);
    }
}