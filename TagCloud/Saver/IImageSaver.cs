using System.Drawing;
using TagCloud.Data;

namespace TagCloud.Saver
{
    public interface IImageSaver
    {
        Result<None> Save(Image image, string fileName);
    }
}