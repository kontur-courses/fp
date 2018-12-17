using System.Collections.Generic;
using System.Linq;
using ResultOf;

namespace TagsCloudVisualization.WordsProcessing
{
    public class BoringWordsFilter : IFilter
    {
        private readonly IWordsProvider boringWordsProvider;
        
        public BoringWordsFilter(IWordsProvider boringWordsProvider)
        {
            this.boringWordsProvider = boringWordsProvider;
        }

        public Result<IEnumerable<string>> FilterWords(IEnumerable<string> words)
        {
            return boringWordsProvider
                .Provide()
                .Then(boringWords => words.Where(word => !boringWords.Contains(word)));
        }
    }
}