using System.Collections.Generic;
using ResultOf;

namespace TagsCloudVisualization.WordsProcessing
{
    public static class EnumerableIFilterExtensions
    {
        public static Result<IEnumerable<string>> FilterWords(this IEnumerable<IFilter> filters,
            IEnumerable<string> words)
        {
            var filteredWords = Result.Ok(words);
            foreach (var filter in filters)
            {
                filteredWords = filter.FilterWords(filteredWords.Value);
                if (!filteredWords.IsSuccess) break;
            }

            return filteredWords;
        }
    }
}