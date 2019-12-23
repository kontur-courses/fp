using System.Collections.Generic;
using TagsCloud.ErrorHandler;

namespace TagsCloud.Visualization
{
    public interface ILayouter
    {
        Result<IEnumerable<Tag.Tag>> GetTags(Dictionary<string, int> wordFrequency);
    }
}