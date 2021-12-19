using System.Collections.Generic;

namespace TagsCloudContainer.WordFilters
{
    public interface IWordsFilter
    {
        Result<List<string>> Filter(IEnumerable<string> words);
    }
}