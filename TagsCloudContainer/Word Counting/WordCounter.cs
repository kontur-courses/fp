using System.Collections.Generic;
using TagsCloudContainer.ResultInfrastructure;

namespace TagsCloudContainer.Word_Counting
{
    public class WordCounter : IWordCounter
    {
        private readonly IWordFilter filter;
        private readonly IWordNormalizer normalizer;

        public WordCounter(IWordFilter filter, IWordNormalizer normalizer)
        {
            this.filter = filter;
            this.normalizer = normalizer;
        }

        public Result<Dictionary<string, int>> CountWords(IEnumerable<string> words)
        {
            var resultDictionary = new Dictionary<string, int>();
            foreach (var word in words)
            {
                var normalizedWordResult = normalizer.Normalize(word);
                if (!normalizedWordResult.IsSuccess)
                    return Result.Fail<Dictionary<string, int>>(
                        $"Cannot normalize word {word} {normalizedWordResult.Error}");
                var normalizedWord = normalizedWordResult.Value;
                if (filter.IsExcluded(normalizedWord))
                    continue;
                if (resultDictionary.ContainsKey(normalizedWord))
                    resultDictionary[normalizedWord]++;
                else
                    resultDictionary[normalizedWord] = 1;
            }

            return Result.Ok(resultDictionary);
        }
    }
}