using System.Collections.Generic;
using System.Linq;
using TagsCloudVisualization.Storages;

namespace TagsCloudVisualization.Frequency
{
    internal class FrequencyCounter : IFrequencyCounter
    {
        public IEnumerable<string> Elements { get; set; }

        public FrequencyCounter(IWordStorage storage)
        {
            Elements = storage.Words;
        }

        public Result<Dictionary<string, int>> GetFrequency()
        {
            if (Elements is null)
                return Result.Fail<Dictionary<string, int>>("The word store is empty. Please check your word source");

            var frequency = new Dictionary<string, int>();
            foreach (var element in Elements)
            {
                if (!frequency.ContainsKey(element))
                {
                    frequency.Add(element, 1);
                }
                else
                {
                    frequency[element]++;
                }
            }

            return frequency;
        }
    }
}
