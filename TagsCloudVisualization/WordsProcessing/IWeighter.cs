using System.Collections.Generic;
using ResultOf;

namespace TagsCloudVisualization.WordsProcessing
{
    public interface IWeighter
    {
        Result<IEnumerable<WeightedWord>> WeightWords(IEnumerable<string> words);
    }
}