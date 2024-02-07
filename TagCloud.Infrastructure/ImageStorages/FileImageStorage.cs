using System.Drawing;
using System.Drawing.Imaging;

public class FileImageStorage : IImageStorage
{
    public Result<None> Save(Bitmap image, string path)
    {
        return Result.OfAction(() => image.Save(path, ImageFormat.Jpeg))
            .RefineError($"Can`t save image with path {path}");
    }
}