using System.Collections.Generic;
using System.Drawing;
using TagsCloud.Visualization.ColorGenerators;
using TagsCloud.Visualization.Models;

namespace TagsCloud.Visualization.LayoutContainer
{
    public class TagsContainer : ILayoutContainer<Tag>
    {
        public IReadOnlyList<Tag> Items { get; init; }
        public Size Size { get; init; }
        public Point Center { get; init; }

        public void Draw(Graphics graphics, IColorGenerator colorGenerator)
        {
            foreach (var (word, fontDecorator, rectangle) in Items)
            {
                var drawFormat = new StringFormat
                {
                    Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center
                };
                using var brush = new SolidBrush(colorGenerator.Generate());
                using var font = fontDecorator.Build();
                graphics.DrawString(word.Content,
                    font, brush, rectangle, drawFormat);
            }
        }
    }
}