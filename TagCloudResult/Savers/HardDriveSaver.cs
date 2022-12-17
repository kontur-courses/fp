using System.Drawing;
using System.Drawing.Imaging;

namespace TagCloudResult.Savers;

public class HardDriveSaver : IBitmapSaver
{
    public void Save(Bitmap bitmap, string filename, ImageFormat format)
    {
        filename = $"{filename}.{format}";
        bitmap.Save(filename, format);
    }
}