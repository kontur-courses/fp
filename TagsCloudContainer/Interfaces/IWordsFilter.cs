using System.Collections.Generic;
using ResultOf;

namespace TagsCloudContainer.Interfaces
{
    public interface IWordsFilter
    {
        Result<IEnumerable<string>> FilterWords(IEnumerable<string> words);
    }
}