using System.Drawing;
using System.Drawing.Imaging;

namespace TagCloudResult.Savers;

public interface IBitmapSaver
{
    public void Save(Bitmap bitmap, string name, ImageFormat format);
}