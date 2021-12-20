using System.Drawing;
using System.IO;

namespace CTV.Common.ImageSavers
{
    public interface IImageSaver
    {
        void Save(Bitmap image, Stream outputStream);
    }
}