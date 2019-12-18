using System.Collections.Generic;
using System.Linq;
using TagCloudResult;
using TagsCloudTextProcessing.Formatters;

namespace TagsCloudTextProcessing.Filters
{
    public class ExcludeFromListFilter : IWordsFilter
    {
        private readonly HashSet<string> wordsToExcludeHashSet;

        public ExcludeFromListFilter(IEnumerable<string> wordsToExclude)
        {
            wordsToExclude = ConvertToUnifiedFormat(wordsToExclude).GetValueOrThrow();
            wordsToExcludeHashSet = new HashSet<string>(wordsToExclude);
        }

        private static Result<IEnumerable<string>> ConvertToUnifiedFormat(IEnumerable<string> wordsToExclude)
        {
           return new LowercaseFormatter().Format(wordsToExclude);
        }

        public Result<IEnumerable<string>> Filter(IEnumerable<string> inputWords)
        {
            var formattedWords = ConvertToUnifiedFormat(inputWords);
            return formattedWords.IsSuccess 
                ? formattedWords.Then(w => w.Where(words => !wordsToExcludeHashSet.Contains(words))) 
                : Result.Fail<IEnumerable<string>>(formattedWords.Error);
        }
    }
}