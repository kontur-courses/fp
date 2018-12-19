using System.Collections.Generic;
using TagsCloudContainer.Tags;

namespace TagsCloudContainer.CloudDrawers
{
    public interface ICloudDrawer
    {
        Result<None> Draw(IEnumerable<Tag> tagsCloud);
    }
}