using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudVisualization.Common.ErrorHandling;
using TagsCloudVisualization.Common.Layouters;
using TagsCloudVisualization.Common.Settings;
using TagsCloudVisualization.Common.Tags;

namespace TagsCloudVisualization.Common.TagCloudPainters
{
    public class TagCloudPainter : ITagCloudPainter
    {
        private readonly ILayouter layouter;
        private readonly ICanvasSettings settings;

        public TagCloudPainter(ILayouter layouter, ICanvasSettings settings)
        {
            this.layouter = layouter;
            this.settings = settings;
        }

        public Result<Bitmap> Paint(IEnumerable<Tag> tags)
        {
            layouter.Clear();
            
            var bitmap = new Bitmap(settings.Width, settings.Height);
            using var graphics = Graphics.FromImage(bitmap);
            graphics.Clear(settings.BackgroundColor);

            foreach (var tag in tags)
            {
                var size = graphics.MeasureString(tag.Text, tag.Style.Font);
                var rect = layouter.PutNextRectangle(
                    new Size((int) Math.Ceiling(size.Width),
                        (int) Math.Ceiling(size.Height)));
                var brush = new SolidBrush(tag.Style.ForegroundColor);
                graphics.DrawString(tag.Text, tag.Style.Font, brush, rect);
            }

            return IsLayoterOverlaped()
                ? Result.Fail<Bitmap>("Генерируемое изображение вышло за пределы холста.")
                : bitmap.AsResult();
        }

        private bool IsLayoterOverlaped()
        {
            var union = layouter.Rects.Aggregate(Rectangle.Union);
            return union.Left < 0 ||
                   union.Top < 0 ||
                   union.Right > settings.Width ||
                   union.Bottom > settings.Height;
        }
    }
}