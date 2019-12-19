using ResultOf;
using System.Collections.Generic;

namespace TagsCloudContainer.WordProcessing.Filtering
{
    public interface IWordFilter
    {
        Result<IEnumerable<string>> FilterWords(IEnumerable<string> words);
    }
}