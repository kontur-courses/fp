using System.Collections.Generic;
using TagsCloudResult;

namespace TagsCloudContainer.WordsFilter
{
    public interface IFilteredWords
    {
        Dictionary<string, int> FilteredWordsList { get; }
    }
}
