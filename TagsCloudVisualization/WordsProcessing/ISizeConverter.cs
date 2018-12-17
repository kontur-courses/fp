using System.Collections.Generic;
using ResultOf;

namespace TagsCloudVisualization.WordsProcessing
{
    public interface ISizeConverter
    {
        Result<IEnumerable<SizedWord>> Convert(IEnumerable<WeightedWord> weightedWords);
    }
}