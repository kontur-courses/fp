using System.Collections.Generic;
using System.Collections.ObjectModel;
using ResultOf;

namespace TagsCloudContainer.Filtering
{
    public interface IWordsFilter
    {
        Result<ReadOnlyCollection<string>> Filter(IEnumerable<string> words);
    }
}