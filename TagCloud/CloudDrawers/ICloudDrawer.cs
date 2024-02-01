using System.Drawing;
using TagCloud;

namespace TagCloudTests;

public interface ICloudDrawer
{
    Result<None> Draw(List<TextRectangle> rectangle);
}