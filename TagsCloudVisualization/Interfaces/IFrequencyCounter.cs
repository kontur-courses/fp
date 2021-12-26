using System.Collections.Generic;

namespace TagsCloudVisualization.Interfaces
{
    public interface IFrequencyCounter
    {
        Result<Dictionary<string, int>> GetFrequencyDictionary(IEnumerable<string> words);
    }
}