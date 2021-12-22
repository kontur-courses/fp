using System.Collections.Generic;
using System.Linq;
using TagCloud.selectors;

namespace TagCloud.repositories
{
    public class WordHelper : IWordHelper
    {
        private readonly IFilter<string> filter;
        private readonly IConverter<IEnumerable<string>> converter;

        public WordHelper(IFilter<string> filter, IConverter<IEnumerable<string>> converter)
        {
            this.filter = filter;
            this.converter = converter;
        }

        public IEnumerable<WordStatistic> GetWordStatistics(IEnumerable<string> words)
        {
            var statistics = new Dictionary<string, int>();
            foreach (var word in words)
            {
                if (!statistics.ContainsKey(word))
                    statistics.Add(word, 0);
                statistics[word]++;
            }

            return statistics.Select(s => new WordStatistic(s.Key, s.Value))
                .OrderBy(s => s.Count);
        }

        public Result<List<string>> FilterWords(IEnumerable<string> words) 
            => Result.Of(() => filter.Filter(words).ToList(), ResultErrorType.FilterError);

        public Result<List<string>> ConvertWords(IEnumerable<string> words) 
            => Result.Of(() => converter.Convert(words).ToList(), ResultErrorType.ConverterError);
    }
}