using System.Drawing;
using ResultOf;

namespace TagCloud.Infrastructure
{
    public interface IImageFormat
    {
        Result<None> SaveImage(Bitmap bitmap, string filePath);
    }
}
