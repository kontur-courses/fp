using System.Drawing;
using FunctionalTools;

namespace TagsCloudGenerator.Saver
{
    public interface IImageSaver
    {
        Result<None> Save(Bitmap bitmap);
    }
}