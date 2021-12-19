using System.Collections.Generic;

namespace TagsCloudVisualization.Infrastructure
{
    public interface IWordSelector
    {
        IEnumerable<string> GetWords(string text);
    }
}