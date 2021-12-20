using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TagsCloud.Visualization.Extensions;
using TagsCloud.Visualization.Utils;
using TagsCloud.Visualization.WordsFilter;

namespace TagsCloud.Visualization.WordsParser
{
    public class WordsParser : IWordsParser
    {
        private const string WordsPattern = @"\W+";
        private readonly IEnumerable<IWordsFilter> wordsFilters;

        public WordsParser(IEnumerable<IWordsFilter> wordsFilters) => this.wordsFilters = wordsFilters;

        public Result<Dictionary<string, int>> CountWordsFrequency(string text)
        {
            return text.AsResult()
                .Validate(t => !string.IsNullOrEmpty(t), nameof(text))
                .Then(ConstructDictionary);
        }

        private Dictionary<string, int> ConstructDictionary(string text)
        {
            return Regex.Split(text.ToLower(), WordsPattern)
                .Where(word => word.Length > 1 && wordsFilters.All(x => x.IsWordValid(word)))
                .GroupBy(word => word)
                .ToDictionary(x => x.Key, x => x.Count());
        }
    }
}