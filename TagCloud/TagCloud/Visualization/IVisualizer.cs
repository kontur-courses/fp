using System.Drawing;
using TagCloud.ErrorHandling;

namespace TagCloud
{
    public interface IVisualizer
    {
        Result<Bitmap> CreateBitMap(int width, int height, Color[] colors, string fontFamily);
    }
}