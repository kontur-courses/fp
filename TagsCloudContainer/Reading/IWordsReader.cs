using System.Collections.Generic;
using System.Collections.ObjectModel;
using ResultOf;

namespace TagsCloudContainer.Reading
{
    public interface IWordsReader
    {
        Result<ReadOnlyCollection<string>> ReadWords(string inputPath);
    }
}