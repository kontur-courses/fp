using System.Collections.Generic;
using System.Drawing;
using TagsCloud.TagGenerators;
using TagsCloud.ErrorHandling;

namespace TagsCloud.Interfaces
{
    public interface ITagCloudGenerator
    {
        Result<IEnumerable<(Tag tag, Rectangle position)>> GenerateTagCloud(IEnumerable<Tag> allTags);
    }
}
