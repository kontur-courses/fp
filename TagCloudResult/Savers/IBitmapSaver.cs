using System.Drawing;
using System.Drawing.Imaging;
using ResultOfTask;

namespace TagCloudResult.Savers;

public interface IBitmapSaver
{
    public Result<None> Save(Bitmap bitmap, string name, ImageFormat format);
}