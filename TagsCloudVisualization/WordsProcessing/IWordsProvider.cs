using System.Collections.Generic;
using ResultOf;

namespace TagsCloudVisualization.WordsProcessing
{
    public interface IWordsProvider
    {
        Result<IEnumerable<string>> Provide();
    }
}