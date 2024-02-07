using System.Drawing;
using ResultLibrary;
using TagsCloudPainter.Tags;

namespace TagsCloudPainter.CloudLayouter;

public interface ICloudLayouter : IResetable
{
    Result<Rectangle> PutNextTag(Tag tag);
    TagsCloud GetCloud();
    void PutTags(List<Tag> tags);
}