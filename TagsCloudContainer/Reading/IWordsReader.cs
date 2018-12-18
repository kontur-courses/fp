using System.Collections.Generic;
using ResultOf;

namespace TagsCloudContainer.Reading
{
    public interface IWordsReader
    {
        Result<List<string>> ReadWords(string inputPath);
    }
}