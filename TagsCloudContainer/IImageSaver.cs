using System.Drawing;

namespace TagsCloudContainer
{
    public interface IImageSaver
    {
        string Save(Bitmap bitmap);
    }
}