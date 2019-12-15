using System.Drawing;
using CSharpFunctionalExtensions;

namespace TagsCloudLibrary.Writers
{
    public interface IImageWriter
    {
        Result WriteBitmapToFile(Bitmap bitmap, string fileName);
    }
}