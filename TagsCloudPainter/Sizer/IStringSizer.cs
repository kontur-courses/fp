using System.Drawing;
using ResultLibrary;

namespace TagsCloudPainter.Sizer;

public interface IStringSizer
{
    Result<Size> GetStringSize(string value, FontFamily fontFamily, float fontSize);
}