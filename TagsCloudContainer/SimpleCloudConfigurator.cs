using System;
using System.Collections.Generic;
using System.Linq;

namespace TagsCloudContainer
{
    public class SimpleCloudConfigurator : ICloudConfigurator
    {
        private readonly Result<Dictionary<string, int>> wordsWithFrequency;

        public SimpleCloudConfigurator(IWordsPreprocessor preprocessor)
        {
            wordsWithFrequency = preprocessor.GetWordsFrequency();
        }

        public Result<IEnumerable<KeyValuePair<string, int>>> ConfigureCloud()
        {
            var fontSize = 8;
            var maxFontSize = 60;
            var multiplier = 1;
            var maxWordsNumber = 200;

            if (!wordsWithFrequency.IsSuccess)
                return Result.Fail<IEnumerable<KeyValuePair<string, int>>>(wordsWithFrequency.Error);

            return Result.Ok(wordsWithFrequency.Value
                .OrderByDescending(kv => kv.Value)
                .Take(maxWordsNumber)
                .Select(kv => new KeyValuePair<string, int>(kv.Key, Math.Min(kv.Value * multiplier + fontSize, maxFontSize))));
        }
    }
}
