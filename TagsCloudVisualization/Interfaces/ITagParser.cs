using System.Collections.Generic;

namespace TagsCloudVisualization.Interfaces
{
    public interface ITagParser
    {
        Result<IEnumerable<Tag>> ParseTags(IDictionary<string, int> words);
    }
}