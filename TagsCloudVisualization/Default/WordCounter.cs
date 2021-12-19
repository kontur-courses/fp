using System.Collections.Generic;
using System.Linq;
using TagsCloudVisualization.Infrastructure;

namespace TagsCloudVisualization.Default
{
    public class WordCounter : ITokenWeigher
    {
        public Result<Token[]> Evaluate(IEnumerable<string> words, int maxTokenCount)
        {
            return Result.Ok(words.GroupBy(x => x)
                .Select(word => new Token(word.Key, word.Count()))
                .OrderByDescending(t => t.Weight)
                .Take(maxTokenCount)
                .ToArray());
        }
    }
}