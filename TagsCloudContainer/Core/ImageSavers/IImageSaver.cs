using System.Drawing;
using ResultOf;

namespace TagsCloudContainer.Core.ImageSavers
{
    interface IImageSaver
    {
        Result<None> Save(string pathImage, Bitmap bitmap, string format);
    }
}