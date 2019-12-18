using System.Collections.Generic;
using ResultOf;

namespace TagsCloudContainer.WordProcessing.Filtering
{
    public interface IWordFilter
    {
        Result<IEnumerable<string>> FilterWords(IEnumerable<string> words);
    }
}