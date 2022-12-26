using System.Collections.Generic;

namespace TagsCloudVisualization.Frequency
{
    public interface IFrequencyCounter
    {
        public IEnumerable<string> Elements { get; set; }
        public Result<Dictionary<string, int>> GetFrequency();
    }
}
