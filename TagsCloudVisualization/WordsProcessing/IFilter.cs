using System.Collections.Generic;
using ResultOf;

namespace TagsCloudVisualization.WordsProcessing
{
    public interface IFilter
    {
        Result<IEnumerable<string>> FilterWords(IEnumerable<string> words);
    }
}