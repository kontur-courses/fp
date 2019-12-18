using System.Collections.Generic;
using ResultOf;

namespace TagsCloudContainer.Interfaces
{
    public interface IWordsCounter
    {
        Result<Dictionary<string, int>> CountWords(IEnumerable<string> arr);
    }
}