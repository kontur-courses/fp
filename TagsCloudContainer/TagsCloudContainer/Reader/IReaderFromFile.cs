using System.Collections.Generic;
using ResultOf;

namespace TagsCloudContainer.Reader
{
    public interface IReaderFromFile
    {
        Result<IEnumerable<string>> GetWordsSet(string path);
    }
}