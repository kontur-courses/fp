using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
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
            var wordsCount = parser
                .CountWordsInFile(pathToSource)
                .RefineError("Источник слов");
            var customBoringWords = parser
                .FindWordsInFile(pathToCustomBoringWords)
                .RefineError("Источник скучных слов");
            var notBoringTypes = new[] { "сущ", "прил", "гл" };

            return !wordsCount.IsSuccess || !customBoringWords.IsSuccess ? 
                Result.Fail<Dictionary<string, int>>(wordsCount.Error + customBoringWords.Error) 
                : Result.Ok(wordsCount.Value
                    .Where(pair =>
                        !customBoringWords.Value.Contains(pair.Key) && 
                        notBoringTypes.Any(type =>
                            morph.GetGrams(pair.Key)["чр"].Contains(type)))
                    .OrderByDescending(pair => pair.Value)
                    .ToDictionary(e => e.Key, e => e.Value));

        }
    }
}
