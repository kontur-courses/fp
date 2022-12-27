using System.Drawing;
using TagCloudPainter.Common;
using TagCloudPainter.ResultOf;

namespace TagCloudPainter.Painters;

public interface ICloudPainter
{
    Result<Bitmap> PaintTagCloud(IEnumerable<Tag> tags);
}