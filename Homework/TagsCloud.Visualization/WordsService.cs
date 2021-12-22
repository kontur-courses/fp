using System.Collections.Generic;
using System.Linq;
using TagsCloud.Visualization.Extensions;
using TagsCloud.Visualization.Models;
using TagsCloud.Visualization.TextProviders;
using TagsCloud.Visualization.Utils;
using TagsCloud.Visualization.WordsParsers;

namespace TagsCloud.Visualization
{
    public class WordsService : IWordsService
    {
        private readonly IWordsParser wordsParser;

        public WordsService(IWordsParser wordsParser) => this.wordsParser = wordsParser;

        public Result<Word[]> GetWords(ITextProvider textProvider)
        {
            return textProvider.Read()
                .Then(text => wordsParser.CountWordsFrequency(text))
                .Then(TransformWords)
                .Then(x => x.ToArray());
        }

        private static IEnumerable<Word> TransformWords(IEnumerable<Word> parsedWords)
            => parsedWords
                .Where(x => x.Count > 1)
                .OrderByDescending(x => x.Count);
    }
}