using System.Drawing;
using TagCloud.ErrorHandling;

namespace TagCloud.Visualization
{
    public interface IVisualizer
    {
        Result<Bitmap> CreateBitMap(int width, int height, Color[] colors, string fontFamily);
    }
}