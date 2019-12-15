using System.Collections.Generic;
using System.Linq;
using TagsCloudVisualization.ErrorHandling;

namespace TagsCloudVisualization.TextFilters
{
    public class BoringWordsFilter : ITextFilter
    {
        private readonly HashSet<string> boringWords;

        public BoringWordsFilter(IEnumerable<string> boringWords)
        {
            this.boringWords = boringWords.ToHashSet();
        }

        public Result<IEnumerable<string>> FilterWords(IEnumerable<string> words)
        {
            return Result.Of(() => words.Where(word => !boringWords.Contains(word)));
        }
    }
}