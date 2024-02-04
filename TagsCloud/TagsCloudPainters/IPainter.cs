using System.Drawing;
using TagsCloud.Entities;
using TagsCloud.Result;

namespace TagsCloud.TagsCloudPainters;

public interface IPainter
{
    public Result<None> DrawCloud(IEnumerable<Tag> tags, Size imageSize);
}