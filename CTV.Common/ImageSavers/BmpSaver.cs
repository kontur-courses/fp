using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace CTV.Common.ImageSavers
{
    public class BmpSaver: IImageSaver
    {
        public void Save(Bitmap image, Stream outputStream)
        {
            image.Save(outputStream, ImageFormat.Bmp);
        }
    }
}