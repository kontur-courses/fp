using System.Drawing;

namespace TagsCloudVisualization.Infrastructure
{
    public interface ICloudMaker
    {
        RectangleF PutRectangle(SizeF rectangleSize);
    }
}