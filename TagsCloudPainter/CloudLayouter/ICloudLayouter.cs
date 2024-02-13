using System.Drawing;
using ResultLibrary;
using TagsCloudPainter.Tags;

namespace TagsCloudPainter.CloudLayouter;

public interface ICloudLayouter : IResetable
{
    TagsCloud PutTags(List<Tag> tags);
}