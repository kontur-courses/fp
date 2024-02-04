using System.Drawing;
using ResultLibrary;

namespace TagsCloudPainter.Drawer;

public interface ICloudDrawer
{
    public Result<Bitmap> DrawCloud(TagsCloud cloud, int imageWidth, int imageHeight);
}