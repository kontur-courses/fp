using System.Collections.Generic;
using TagsCloud.ErrorHandler;

namespace TagsCloud.Visualization
{
    public interface ILayouter
    {
        IEnumerable<Result<Tag.Tag>> GetTags(Dictionary<string, int> wordFrequency);
    }
}