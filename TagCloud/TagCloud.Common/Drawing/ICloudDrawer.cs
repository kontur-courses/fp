using System.Drawing;
using ResultOf;

namespace TagCloud.Common.Drawing;

public interface ICloudDrawer
{
    Result<Bitmap> DrawCloud(IEnumerable<Tag> tags);
}