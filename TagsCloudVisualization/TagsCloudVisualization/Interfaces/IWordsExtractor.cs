using System.Collections.Generic;
using TagsCloudVisualization.Settings;

namespace TagsCloudVisualization.Interfaces
{
    public interface IWordsExtractor
    {
        Result<List<string>> Extract(string path, IWordsExtractorSettings settings);
    }
}