using System.Drawing;
using System.IO;
using FunctionalProgrammingInfrastructure;

namespace CTV.Common.ImageSavers
{
    public interface IImageSaver
    {
        Result<None> Save(Bitmap image, Stream outputStream);
    }
}