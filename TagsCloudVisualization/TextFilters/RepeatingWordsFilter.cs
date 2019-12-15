using System.Collections.Generic;
using System.Linq;
using TagsCloudVisualization.ErrorHandling;

namespace TagsCloudVisualization.TextFilters
{
    public class RepeatingWordsFilter : ITextFilter
    {
        public Result<IEnumerable<string>> FilterWords(IEnumerable<string> words)
        {
            return Result.Of(words.Distinct);
        }
    }
}