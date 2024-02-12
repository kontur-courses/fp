using System.Drawing;
using ResultLibrary;
using TagsCloudPainter.Tags;

namespace TagsCloudPainter.Sizer;

public interface ITagSizer
{
    public Result<Size> GetTagSize(Tag tag, FontFamily tagFont);
}