using System.Drawing;

namespace TagsCloudVisualization.TagCloudLayouter
{
    public interface ICloudLayouter
    {
        public Result<Rectangle> PutNextRectangle(Size rectangleSize);
        void ClearLayout();
    }
}