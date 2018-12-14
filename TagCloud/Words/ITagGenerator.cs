using System.Collections.Generic;
using TagCloud.TagCloudVisualization.Analyzer;

namespace TagCloud.Words
{
    public interface ITagGenerator
    {
        Result<List<Tag>> GetTags(IEnumerable<string> words);
    }
}