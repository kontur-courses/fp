using System;
using System.Collections.Generic;
using System.Linq;
using ResultOf;

namespace TagsCloudContainer.Preprocessing
{
    public class FrequencyCounter
    {
        private IEnumerable<WordInfo> OrderWordFrequencies(Dictionary<string, int> frequencies)
        {
            return frequencies
                .OrderByDescending(kv => kv.Value)
                .Select(kv => new WordInfo
                {
                    Word = kv.Key,
                    Frequency = kv.Value
                });
        }

        private IEnumerable<WordInfo> CountFrequencies(IEnumerable<string> words)
        {
            if (words == null)
                throw new ArgumentNullException(nameof(words), "word must be not null");
            var wordsFrequencies = new Dictionary<string, int>();
            foreach (var word in words)
            {
                wordsFrequencies.TryGetValue(word, out var value);
                wordsFrequencies[word] = value + 1;
            }

            return OrderWordFrequencies(wordsFrequencies);
        }

        public Result<IEnumerable<WordInfo>> CountWordFrequencies(IEnumerable<string> words)
        {
            return Result.Of(() => CountFrequencies(words));
        }
    }
}