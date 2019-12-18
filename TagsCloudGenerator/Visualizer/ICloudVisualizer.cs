using System.Drawing;
using TagsCloudGenerator.CloudLayouter;

namespace TagsCloudGenerator.Visualizer
{
    public interface ICloudVisualizer
    {
        Result<Bitmap> Draw(Cloud cloud);
    }
}