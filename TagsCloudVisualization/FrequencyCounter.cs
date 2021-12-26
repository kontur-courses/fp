using System.Collections.Generic;
using TagsCloudVisualization.Extensions;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudVisualization
{
    public class FrequencyCounter : IFrequencyCounter
    {
        public Result<Dictionary<string, int>> GetFrequencyDictionary(IEnumerable<string> words)
        {
            if (words == null)
                return Result.Fail<Dictionary<string, int>>("Words was null");

            var freqDictionary = new Dictionary<string, int>();

            foreach (var word in words)
                freqDictionary.AddOrUpdate(word, 1, value => value + 1);

            return freqDictionary;
        }
    }
}