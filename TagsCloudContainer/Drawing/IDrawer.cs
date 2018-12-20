using System.Drawing.Imaging;
using TagsCloudContainer.Layout;

namespace TagsCloudContainer.Drawing
{
    public interface IDrawer
    {
        byte[] Draw(IWordLayout layout, ImageSettings settings, ImageFormat format);
    }
}