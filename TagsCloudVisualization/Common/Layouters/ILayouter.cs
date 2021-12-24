using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization.Common.Layouters
{
    public interface ILayouter
    {
        public IEnumerable<Rectangle> Rects { get; }

        public Rectangle PutNextRectangle(Size rectangleSize);

        public void Clear();
    }
}