using TagsCloudContainer.Core.Results;
using TagsCloudContainer.Core.WordsParser.Interfaces;

namespace TagsCloudContainer.Core.WordsParser
{
    public class WordsAnalyzer : IWordsAnalyzer
    {
        private readonly IWordsFilter _filter;
        private readonly IFileReader _fileReader;

        public WordsAnalyzer(IWordsFilter filter, IFileReader fileReader)
        {
            _filter = filter;
            _fileReader = fileReader;
        }

        public Result<Dictionary<string, int>> AnalyzeWords()
        {
            return _fileReader.ReadWords()
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
