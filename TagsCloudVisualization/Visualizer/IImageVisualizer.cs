using System.Drawing;

namespace TagsCloudVisualization.Visualizer
{
    internal interface IImageVisualizer
    {
        public Result<Image> CreateImage();
    }
}
