using System.Collections.Generic;

namespace TagsCloudContainer
{
    public interface ITagCloudBuilder
    {
        Result<IEnumerable<Tag>> GetTagsCloud();
    }
}