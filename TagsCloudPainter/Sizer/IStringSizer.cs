using System.Drawing;

namespace TagsCloudPainter.Sizer;

public interface IStringSizer
{
    Result<Size> GetStringSize(string value, string fontName, float fontSize);
}