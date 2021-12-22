#region

using System.Collections.Generic;

#endregion

namespace TagsCloudVisualization.Interfaces
{
    public interface IFrequencyCounter
    {
        Result<Dictionary<string, int>> GetFrequencyDictionary(IEnumerable<string> words);
    }
}