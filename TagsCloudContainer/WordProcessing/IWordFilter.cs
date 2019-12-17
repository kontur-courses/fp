using System.Collections.Generic;
using ResultOf;

namespace TagsCloudContainer.WordProcessing
{
    public interface IWordFilter
    {
        Result<IEnumerable<string>> Filter(IEnumerable<string> words);
    }
}