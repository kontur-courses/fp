using TagsCloudContainer.Core.Results;
using TagsCloudContainer.Core.WordsParser.Interfaces;

namespace TagsCloudContainer.Core.WordsParser
{
    public class WordsAnalyzer : IWordsAnalyzer
    {
        private readonly IWordsFilter _filter;
        private readonly IWordsReader _wordReader;

        public WordsAnalyzer(IWordsFilter filter, IWordsReader wordReader)
        {
            _filter = filter;
            _wordReader = wordReader;
        }

        public Result<Dictionary<string, int>> AnalyzeWords()
        {
            return _wordReader.ReadWords()
                .Then(words => words.Select(word => word.ToLower()))
                .Then(normalizedWords => _filter.RemoveBoringWords(normalizedWords))
                .Then(CountWords);
        }

        private static Dictionary<string, int> CountWords(IEnumerable<string> words)
        {
            return words.GroupBy(word => word).ToDictionary(group => group.Key, group => group.Count());
        }
    }
}
