using System.Collections.Generic;
using TagsCloudVisualization.Structures;
using ResultPatterLibrary;

namespace TagsCloudVisualization.Filters
{
    public interface IFilter
    {
        bool Filter(Result<WordInfo> wordInfo);
        IEnumerable<string> GetFilteredValues(string valueToFilter);
    }
}
