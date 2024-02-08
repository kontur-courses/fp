using System.Drawing;
using TagsCloud.Entities;

namespace TagsCloud.TagsCloudPainters;

public interface IPainter
{
    public Result<None> DrawCloud(Entities.Cloud cloud);
}