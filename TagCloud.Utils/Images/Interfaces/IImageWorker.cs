using Aspose.Drawing;
using Aspose.Drawing.Imaging;
using TagCloud.Utils.ResultPattern;

namespace TagCloud.Utils.Images.Interfaces;

public interface IImageWorker
{
    public Result<None> SaveImage(Image image, string path, ImageFormat imageFormat, string filename);
}