using System.Drawing;
using System.Drawing.Imaging;
using TagsCloud.ErrorHandling;

namespace TagsCloud.Interfaces
{
    public interface IImageSaver
    {
        Result<None> SaveImage(Image image, string path, ImageFormat formatResult);
    }
}
