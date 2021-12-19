using System.Collections.Generic;

namespace TagsCloudVisualization.Infrastructure
{
    public interface IWordSelector
    {
        Result<IEnumerable<string>> GetWords(string text);
    }
}