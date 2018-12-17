using System.Collections.Generic;
using System.Linq;
using ResultOf;

namespace TagsCloudVisualization.WordsProcessing
{
    public class FrequencyWeighter : IWeighter
    {
        public Result<IEnumerable<WeightedWord>> WeightWords(IEnumerable<string> words)
        {
            return Result.Of(() => words
                .GroupBy(s => s)
                .OrderByDescending(s => s.Count())
                .Select(g => new WeightedWord(g.Key, g.Count())));
        }
    }
}