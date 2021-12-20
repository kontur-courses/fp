using System.Collections.Generic;
using System.Drawing;
using TagsCloud.Visualization.ContainerVisitor;
using TagsCloud.Visualization.Models;

namespace TagsCloud.Visualization.LayoutContainer
{
    public class TagsContainer : ILayoutContainer<Tag>
    {
        public IReadOnlyList<Tag> Items { get; init; }
        public Size Size { get; init; }
        public Point Center { get; init; }

        public void Accept(Graphics graphics, IContainerVisitor visitor)
        {
            visitor.Visit(graphics, this);
        }
    }
}