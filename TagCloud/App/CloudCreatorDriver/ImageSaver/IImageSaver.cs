using System.Drawing;

namespace TagCloud.App.CloudCreatorDriver.ImageSaver;

public interface IImageSaver
{
    Result<None> SaveImage(Bitmap image, string fullFileName);
}