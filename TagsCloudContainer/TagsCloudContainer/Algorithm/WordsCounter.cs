using DeepMorphy;
using TagsCloudContainer.Extensions;
using TagsCloudContainer.Infrastructure;

namespace TagsCloudContainer.Algorithm
{
    public class WordsCounter : IWordsCounter
    {
        private readonly MorphAnalyzer morph;
        private readonly IParser parser;

        public WordsCounter(MorphAnalyzer morph, IParser parser)
        {
            this.morph = morph;
            this.parser = parser;
        }

        public Result<Dictionary<string, int>> CountWords(string pathToSource, string pathToCustomBoringWords)
        {
            var wordsCountRes = parser
                .CountWordsInFile(pathToSource)
                .RefineError("Источник слов");
            var customBoringWordsRes = parser
                .FindWordsInFile(pathToCustomBoringWords)
                .RefineError("Источник скучных слов");

            return !wordsCountRes.IsSuccess || !customBoringWordsRes.IsSuccess ? 
                Result.Fail<Dictionary<string, int>>(wordsCountRes.Error + '\t' + customBoringWordsRes.Error) 
                : Result.Ok(
                    RemoveBoringWords(wordsCountRes.Value, customBoringWordsRes.Value)
                        .OrderByDescending(pair => pair.Value)
                        .ToDictionary(e => e.Key, e => e.Value));
        }

        private IEnumerable<KeyValuePair<string, int>> RemoveBoringWords(Dictionary<string, int> wordsCount,
             HashSet<string> customBoringWords)
        {
            var notBoringTypes = new[] { "сущ", "прил", "гл" };

            return wordsCount
                .Where(pair =>
                    !customBoringWords.Contains(pair.Key) &&
                    notBoringTypes.Any(type =>
                        morph.GetGrams(pair.Key)["чр"].Contains(type)));
        }
    }
}
