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
                .Then(wordsFilters.FilterWords)
                .Then(wordsChangers.ChangeWords);
        }
    }
}