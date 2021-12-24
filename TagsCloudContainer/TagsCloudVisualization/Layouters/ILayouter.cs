using System.Collections.Generic;
using System.Drawing;
using TagsCloudVisualization.ResultOf;

namespace TagsCloudVisualization.Layouters
{
    public interface ILayouter
    {
        public IEnumerable<RectangleF> PlacedRectangles { get; }
        public Result<RectangleF> PutNextRectangle(SizeF rectangleSize);
        public Result<IEnumerable<RectangleF>> PutNextRectangles(IEnumerable<SizeF> rectanglesSizes);
    }
}