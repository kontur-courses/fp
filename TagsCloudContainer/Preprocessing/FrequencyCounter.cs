using System.Collections.Generic;
using System.Linq;
using ResultOf;

namespace TagsCloudContainer.Preprocessing
{
    public class FrequencyCounter
    {
        public Result<IEnumerable<WordInfo>> CountWordFrequencies(IEnumerable<string> words)
        {
            if (words == null)
                return Result.Fail<IEnumerable<WordInfo>>("word must be not null");
            var wordsFrequencies = new Dictionary<string, int>();
            foreach (var word in words)
            {
                wordsFrequencies.TryGetValue(word, out var value);
                wordsFrequencies[word] = value + 1;
            }

            return OrderWordFrequencies(wordsFrequencies).AsResult();
        }

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
    }
}