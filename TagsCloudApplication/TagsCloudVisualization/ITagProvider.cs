using System.Collections.Generic;
using TextConfiguration;

namespace TagsCloudVisualization
{
    public interface ITagProvider
    {
        Result<List<CloudTag>> ReadCloudTags(string filePath);
    }
}
