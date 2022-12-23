using System.Drawing;

namespace TagCloud.App.CloudCreatorDriver.RectanglesLayouters;

public interface ICloudLayouter
{
    Result<List<Rectangle>> GetLaidRectangles(IEnumerable<Size> sizes, ICloudLayouterSettings layouterSettings);
}