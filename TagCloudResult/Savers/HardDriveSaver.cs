using System.Drawing;
using System.Drawing.Imaging;
using ResultOfTask;

namespace TagCloudResult.Savers;

public class HardDriveSaver : IBitmapSaver
{
    public Result<None> Save(Bitmap bitmap, string filename, ImageFormat format)
    {
        return Result.OfAction(() => bitmap.Save(filename, format));
    }
}