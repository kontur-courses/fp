using System.Collections.Generic;
using System.Linq;
using ResultOf;
using TagCloud.Infrastructure;

namespace TagCloud.WordsProcessing
{
    public class WordProcessor : IWordProcessor
    {
        private readonly IWordSelector wordSelector;
        private readonly IWordCounter wordCounter;

        public WordProcessor(IWordSelector wordSelector, IWordCounter wordCounter)
        {
            this.wordSelector = wordSelector;
            this.wordCounter = wordCounter;
        }

        public Result<IEnumerable<Word>> PrepareWords(IEnumerable<string> rawWords)
        {
            var convertedWords = rawWords
                .Select(rawWord => rawWord.ToLower())
                .Select(word => new Word(word))
                .ToList();
            if (convertedWords.Count == 0)
                return Result.Fail<IEnumerable<Word>>("No words were given");
            var countedWords = wordCounter.GetCountedWords(convertedWords);
            var selectedWords = countedWords
                .Where(word => wordSelector.IsSelectedWord(word));
            return Result.Ok(selectedWords);

        }
    }
}
