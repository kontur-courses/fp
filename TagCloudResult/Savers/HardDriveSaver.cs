using System.Drawing;
using System.Drawing.Imaging;
using ResultOfTask;

namespace TagCloudResult.Savers;

public class HardDriveSaver : IBitmapSaver
{
    public Result<None> Save(Bitmap bitmap, string filename, ImageFormat format)
    {
        try
        {
            bitmap.Save(filename, format);
        }
        catch (Exception e)
        {
            return Result.Fail<None>($"Couldn't save image {filename}.");
        }

        return new Result<None>();
    }
}