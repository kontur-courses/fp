using System.Collections.Generic;
using TagsCloudVisualization.Results;
using TagsCloudVisualization.Structures;

namespace TagsCloudVisualization.Filters
{
    public interface IFilter
    {
        bool Filter(Result<WordInfo> wordInfo);
        IEnumerable<string> GetFilteredValues(string valueToFilter);
    }
}
