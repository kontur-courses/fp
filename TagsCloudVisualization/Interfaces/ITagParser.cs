#region

using System.Collections.Generic;

#endregion

namespace TagsCloudVisualization.Interfaces
{
    public interface ITagParser
    {
        Result<IEnumerable<Tag>> ParseTags(IDictionary<string, int> words);
    }
}