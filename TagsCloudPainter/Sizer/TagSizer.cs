using System.Drawing;
using ResultLibrary;
using TagsCloudPainter.Tags;

namespace TagsCloudPainter.Sizer;

public class TagSizer : ITagSizer
{
    private readonly IStringSizer stringSizer;

    public TagSizer(IStringSizer stringSizer)
    {
        this.stringSizer = stringSizer ?? throw new ArgumentNullException(nameof(stringSizer));
    }

    public Result<Size> GetTagSize(Tag tag, FontFamily tagFont)
    {
        var tagSize = stringSizer.GetStringSize(tag.Value, tagFont, tag.FontSize);
        tagSize = tagSize.Then(tagSize => tagSize.Height <= 0 || tagSize.Width <= 0
            ? Result.Fail<Size>("Either width or height of rectangle size is not possitive")
            : tagSize);

        return tagSize;
    }
}