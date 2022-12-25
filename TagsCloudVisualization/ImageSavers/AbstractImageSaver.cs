using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudVisualization.ImageSavers;

public abstract class AbstractImageSaver
{
    public Result<None> Save(string fullpath, Bitmap image)
    {
        return Result.Of(() => image.Save(Path.ChangeExtension(fullpath, Extension), Format), "Can't save image");
    }

    protected abstract string Extension { get; }
    protected abstract ImageFormat Format { get; }
}