using System;
using System.Collections.Generic;
using System.Drawing;
using TagCloud.Creators;
using TagCloud.Layouters;
using TagCloud.ResultMonad;

namespace TagCloud.Visualizers
{
    public class CloudVisualizer : IVisualizer
    {
        private readonly DrawingSettings drawingSettings;

        public CloudVisualizer(DrawingSettings drawingSettings)
        {
            this.drawingSettings = drawingSettings;
        }

        public Result<Bitmap> DrawCloud(IEnumerable<Tag> tags,
            ITagColoringFactory tagColoringFactory)
        {
            var bitmap = new Bitmap(drawingSettings.Width, drawingSettings.Height);
            using var graph = Graphics.FromImage(bitmap);

            var tagColoringAlgorithm = tagColoringFactory
                .Create(drawingSettings.AlgorithmName, drawingSettings.PenColors);

            if (!tagColoringAlgorithm.IsSuccess)
                return Result.Fail<Bitmap>(tagColoringAlgorithm.Error);

            var backgroundBrush = drawingSettings.BackgroundColor;
            graph.Clear(backgroundBrush);

            foreach (var tag in tags)
            {
                if (!tag.ContainingRectangle.InsideSize(bitmap.Size))
                    return Result.Fail<Bitmap>("Tag is out of image");
                var color = tagColoringAlgorithm.Value.GetNextColor();
                using var brush = new SolidBrush(color);
                DrawTag(tag, graph, drawingSettings.Font, brush);
            }

            return bitmap.AsResult();
        }

        private void DrawTag(Tag tag, Graphics graph, Font font, Brush pen)
        {
            using var drawFont = new Font(font.FontFamily, font.Size * tag.Frequency);
            graph.DrawString(tag.Value, drawFont, pen, tag.ContainingRectangle);
        }
    }
}
