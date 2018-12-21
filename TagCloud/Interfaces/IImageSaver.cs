using System.Drawing;

namespace TagCloud.Interfaces
{
    public interface IImageSaver
    {
        Result<None> Save(Image image, string path);
    }
}