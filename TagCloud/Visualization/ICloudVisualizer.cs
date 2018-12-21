using System.Drawing;
using ResultOf;

namespace TagCloud
{
    public interface ICloudVisualizer
    {
        void AddWord(Word word, Rectangle position, Font font);
        Result<Bitmap> CreateImage(Color textColor, Color backgroundColor, Size? size);
    }
}