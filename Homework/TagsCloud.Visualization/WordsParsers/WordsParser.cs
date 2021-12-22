using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TagsCloud.Visualization.Extensions;
using TagsCloud.Visualization.Models;
using TagsCloud.Visualization.Utils;
using TagsCloud.Visualization.WordsFilters;

namespace TagsCloud.Visualization.WordsParsers
{
    public class WordsParser : IWordsParser
    {
        private const string WordsPattern = @"\W+";
        private readonly IEnumerable<IWordsFilter> wordsFilters;

        public WordsParser(IEnumerable<IWordsFilter> wordsFilters) => this.wordsFilters = wordsFilters;

        public Result<IEnumerable<Word>> CountWordsFrequency(string text)
        {
            return text.AsResult()
                .Validate(t => !string.IsNullOrEmpty(t), nameof(text))
                .Then(ConstructWords);
        }

        private IEnumerable<Word> ConstructWords(string text)
        {
            return Regex.Split(text.ToLower(), WordsPattern)
                .Where(word => wordsFilters.All(x => x.IsWordValid(word)))
                .GroupBy(word => word)
                .Select(x => new Word(x.Key, x.Count()));
        }
    }
}