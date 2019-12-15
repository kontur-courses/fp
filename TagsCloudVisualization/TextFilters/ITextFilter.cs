using System.Collections.Generic;
using TagsCloudVisualization.ErrorHandling;

namespace TagsCloudVisualization.TextFilters
{
    public interface ITextFilter
    {
        Result<IEnumerable<string>> FilterWords(IEnumerable<string> words);
    }
}