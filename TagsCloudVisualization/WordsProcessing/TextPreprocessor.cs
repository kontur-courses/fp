using System.Collections.Generic;
using ResultOf;

namespace TagsCloudVisualization.WordsProcessing
{
    public class TextPreprocessor : IWordsProvider
    {
        private readonly IWordsProvider wordsProvider;
        private readonly IFilter[] wordsFilters;
        private readonly IWordsChanger[] wordsChangers;

        public TextPreprocessor(IWordsProvider wordsProvider, IFilter[] wordsFilters, IWordsChanger[] wordsChangers)
        {
            this.wordsProvider = wordsProvider;
            this.wordsFilters = wordsFilters;
            this.wordsChangers = wordsChangers;
        }

        public Result<IEnumerable<string>> Provide()
        {
            return wordsProvider
                .Provide()
                .Then(FilterWords)
                .Then(ChangeWords);
        }

        private Result<IEnumerable<string>> FilterWords(IEnumerable<string> words)
        {
            var filteredWords = Result.Ok(words);
            foreach (var filter in wordsFilters)
            {
                filteredWords = filter.FilterWords(filteredWords.Value);
                if (!filteredWords.IsSuccess) break;
            }

            return filteredWords;
        }
        private Result<IEnumerable<string>> ChangeWords(IEnumerable<string> words)
        {

            var changedWords = Result.Ok(words);
            foreach (var changer in wordsChangers)
            {
                changedWords = changer.ChangeWords(changedWords.Value);
                if (!changedWords.IsSuccess) break;
            }

            return changedWords;
        }
    }
}