using System.Drawing;
using TagCloud.Result;

namespace TagCloud.Interfaces
{
    public interface IImageSaver
    {
        Result<None> Save(Image image, string path);
    }
}