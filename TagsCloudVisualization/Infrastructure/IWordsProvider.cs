using System.Collections.Generic;

namespace TagsCloudVisualization.Infrastructure
{
    public interface IWordsProvider
    {
        Result<IEnumerable<string>> GetWords(string path);
    }
}