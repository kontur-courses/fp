using System.Collections.Generic;
using System.Drawing;
using TagsCloud.ErrorHandling;
using TagsCloud.TagGenerators;

namespace TagsCloud.Interfaces
{
    public interface ITagCloudGenerator
    {
        Result<IEnumerable<(Tag tag, Rectangle position)>> GenerateTagCloud(IEnumerable<Tag> allTags);
    }
}