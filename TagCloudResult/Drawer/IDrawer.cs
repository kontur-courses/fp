using System.Drawing;

namespace TagCloudResult.Drawer
{
    public interface IDrawer
    {
        public Result<Image> GetImage();
    }
}
