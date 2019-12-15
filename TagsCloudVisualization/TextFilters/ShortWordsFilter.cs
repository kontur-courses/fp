using System.Collections.Generic;
using System.Linq;
using TagsCloudVisualization.ErrorHandling;

namespace TagsCloudVisualization.TextFilters
{
    public class ShortWordsFilter : ITextFilter
    {
        private readonly int minLength;

        public ShortWordsFilter(int minLength = 3)
        {
            this.minLength = minLength;
        }

        public Result<IEnumerable<string>> FilterWords(IEnumerable<string> words)
        {
            return Result.Of(() => words.Where(word => word.Length > minLength));
        }
    }
}