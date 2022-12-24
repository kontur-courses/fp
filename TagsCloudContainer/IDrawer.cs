using System.Drawing;

namespace TagsCloudContainer
{
    public interface IDrawer
    {
        Result<Bitmap> DrawImage(string text);
    }
}