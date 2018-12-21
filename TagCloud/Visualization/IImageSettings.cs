using System.Drawing;
using ResultOf;

namespace TagCloud
{
    public interface IImageSettings
    {
        Result<Color> GetTextColor();
        Result<Color> GetBackgroundColor();
        Result<Font> GetFont(float fontSize);
        Result<Size?> GetSize();
    }
}