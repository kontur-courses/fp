using System.Collections.Generic;
using System.Drawing;
using TagsCloud.Visualization.ContainerVisitor;

namespace TagsCloud.Visualization.LayoutContainer
{
    public class RectanglesContainer : ILayoutContainer<Rectangle>
    {
        public IReadOnlyList<Rectangle> Items { get; init; }
        public Size Size { get; init; }
        public Point Center { get; init; }

        public void Accept(Graphics graphics, IContainerVisitor visitor)
        {
            visitor.Visit(graphics, this);
        }
    }
}