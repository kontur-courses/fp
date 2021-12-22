using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization.Interfaces
{
    public interface ICloud
    {
        IReadOnlyList<ITag> Elements { get; }
        PointF Center { get; }

        internal void AddTag(ITag tag);
        public RectangleF GetCloudBoundingRectangle();
    }
}
