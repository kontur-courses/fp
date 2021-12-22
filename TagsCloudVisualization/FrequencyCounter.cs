#region

using System.Collections.Generic;
using TagsCloudVisualization.Extensions;
using TagsCloudVisualization.Interfaces;

#endregion

namespace TagsCloudVisualization
{
    public class FrequencyCounter : IFrequencyCounter
    {
        public Result<Dictionary<string, int>> GetFrequencyDictionary(IEnumerable<string> words)
        {
            if (words == null)
                return new Result<Dictionary<string, int>>("Words was null");

            var freqDictionary = new Dictionary<string, int>();

            foreach (var word in words)
                freqDictionary.AddOrUpdate(word, 1, x => freqDictionary[word] + 1);

            return new Result<Dictionary<string, int>>(null, freqDictionary);
        }
    }
}