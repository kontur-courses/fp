using System.Collections.Generic;
using TagCloudResult;

namespace TagsCloudTextProcessing.Filters
{
    public interface IWordsFilter
    {
        Result<IEnumerable<string>> Filter(IEnumerable<string> inputWords);
    }
}