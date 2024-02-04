using ResultLibrary;
using System.Drawing;

namespace TagsCloudPainter.Sizer;

public class StringSizer : IStringSizer
{
    public Result<Size> GetStringSize(string value, FontFamily fontFamily, float fontSize)
    {
        using var graphics = Graphics.FromHwnd(IntPtr.Zero);
        using var font = new Font(fontFamily, fontSize);
        {
            var size = Result.Of(() => graphics.MeasureString(value, font).ToSize());
            return size;
        }
    }
}