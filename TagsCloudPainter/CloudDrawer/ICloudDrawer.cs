using ResultLibrary;
using System.Drawing;

namespace TagsCloudPainter.Drawer;

public interface ICloudDrawer
{
    public Result<Bitmap> DrawCloud(TagsCloud cloud, int imageWidth, int imageHeight);
}