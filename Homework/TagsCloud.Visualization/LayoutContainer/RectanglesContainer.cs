using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloud.Visualization.ColorGenerators;

namespace TagsCloud.Visualization.LayoutContainer
{
    public class RectanglesContainer : ILayoutContainer<Rectangle>
    {
        public IReadOnlyList<Rectangle> Items { get; init; }
        public Size Size { get; init; }
        public Point Center { get; init; }

        public void Draw(Graphics graphics, IColorGenerator colorGenerator)
        {
            using var pen = new Pen(colorGenerator.Generate());
            graphics.DrawRectangles(pen, Items.ToArray());
        }
    }
}