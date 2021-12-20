using System.Collections.Generic;
using System.Drawing;
using TagsCloud.Visualization.ContainerVisitor;
using TagsCloud.Visualization.Models;

namespace TagsCloud.Visualization.LayoutContainer
{
    public class WordsContainer : ILayoutContainer<WordWithBorder>
    {
        public IReadOnlyList<WordWithBorder> Items { get; init; }
        public Size Size { get; init; }
        public Point Center { get; init; }

        public void Accept(Graphics graphics, IContainerVisitor visitor)
        {
            visitor.Visit(graphics, this);
        }
    }
}