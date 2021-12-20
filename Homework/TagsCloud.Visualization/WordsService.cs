using System.Collections.Generic;
using System.Linq;
using TagsCloud.Visualization.Extensions;
using TagsCloud.Visualization.Models;
using TagsCloud.Visualization.Utils;
using TagsCloud.Visualization.WordsParser;
using TagsCloud.Visualization.WordsReaders;

namespace TagsCloud.Visualization
{
    public class WordsService : IWordsService
    {
        private readonly IWordsParser wordsParser;

        public WordsService(IWordsParser wordsParser) => this.wordsParser = wordsParser;

        public Result<Word[]> GetWords(IWordsProvider wordsProvider)
        {
            return wordsProvider.Read()
                .Then(t => wordsParser.CountWordsFrequency(t))
                .Then(TransformToWords);
        }

        private Word[] TransformToWords(Dictionary<string, int> parsedWords)
            => parsedWords
                .Where(x => x.Value > 1)
                .OrderByDescending(x => x.Value)
                .Select(x => new Word(x.Key, x.Value))
                .ToArray();
    }
}