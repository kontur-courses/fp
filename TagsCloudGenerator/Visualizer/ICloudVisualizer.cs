using System.Drawing;
using FunctionalTools;
using TagsCloudGenerator.CloudLayouter;

namespace TagsCloudGenerator.Visualizer
{
    public interface ICloudVisualizer
    {
        Result<Bitmap> Draw(Cloud cloud);
    }
}