using System.Drawing;

namespace TagsCloudVisualization.CloudLayouter;

public interface ICloudLayouter
{
    Result<RectangleF> PutNextRectangle(SizeF rectangleSize, LayoutOptions options);

    void Reset();
}