using System.Collections.Generic;
using System.Linq;
using ResultOf;
using TagCloud.Factories;

namespace TagCloud
{
    public class WordsHandler : IWordsHandler
    {
        private readonly IBoringWordsFactory boringWordsFactory;

        public WordsHandler(IBoringWordsFactory boringWordsFactory)
        {
            this.boringWordsFactory = boringWordsFactory;
        }

        public Result<Dictionary<string, int>> RemoveBoringWords(Dictionary<string, int> wordsAndCount,
            string pathToBoringWords)
        {
            return boringWordsFactory.GetFromFile(pathToBoringWords)
                .Then(bw => GetWordsDictionaryWithoutBoringWords(wordsAndCount, bw));
        }

        public Result<Dictionary<string, int>> GetWordsAndCount(IEnumerable<string> words)
        {
            return Result.Of(
                    () => words
                        .GroupBy(word => word)
                        .ToDictionary(g => g.Key, g => g.Count())
                )
                .ReplaceError(error =>
                    error.Replace("Значение не может быть неопределенным.\r\n", "Параметр не определен"));
        }

        private Dictionary<string, int> GetWordsDictionaryWithoutBoringWords(Dictionary<string, int> dictionary,
            HashSet<string> boringWords)
        {
            return dictionary.Select(p => new KeyValuePair<string, int>(p.Key.ToLower(), p.Value))
                .Where(p => !boringWords.Contains(p.Key))
                .ToDictionary(p => p.Key, p => p.Value);
        }
    }
}