using System.Drawing;
using TagsCloudPainter.Tags;

namespace TagsCloudPainter.CloudLayouter;

public interface ICloudLayouter : IResetable
{
    Result<Rectangle> PutNextTag(Tag tag);
    TagsCloud GetCloud();
    Result<None> PutTags(List<Tag> tags);
}