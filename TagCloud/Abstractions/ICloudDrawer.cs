using System.Drawing;

namespace TagCloud.Abstractions;

public interface ICloudDrawer
{
    Graphics Graphics { get; }
    DrawerSettings Settings { get; }
    Bitmap Draw(IEnumerable<IDrawableTag> tags);
}