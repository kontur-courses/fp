using System.Drawing;

namespace TagCloud.TagCloudVisualization.Layouter
{
    public interface ICloudLayouter
    {
        void Clear();
        Rectangle PutNextRectangle(Size rectangleSize);
    }
}