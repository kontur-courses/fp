using System.Drawing;
using ResultLibrary;
using TagsCloudPainter.Tags;

namespace TagsCloudPainter.CloudLayouter;

public interface ICloudLayouter : IResetable
{
    Result<Rectangle> PutNextTag(Tag tag);
    TagsCloud GetCloud();
    TagsCloud PutTags(List<Tag> tags);
}