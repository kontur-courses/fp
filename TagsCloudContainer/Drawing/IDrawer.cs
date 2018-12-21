using TagsCloudContainer.Layout;
using TagsCloudContainer.Settings;

namespace TagsCloudContainer.Drawing
{
    public interface IDrawer
    {
        byte[] Draw(IWordLayout layout, ImageSettings settings);
    }
}