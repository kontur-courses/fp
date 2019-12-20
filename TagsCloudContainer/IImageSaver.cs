using System.Drawing;
using ResultOf;

namespace TagsCloudContainer
{
    public interface IImageSaver
    {
        Result<string> Save(Bitmap bitmap);
    }
}