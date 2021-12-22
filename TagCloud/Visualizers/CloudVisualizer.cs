using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagCloud.Creators;
using TagCloud.Layouters;
using TagCloud.ResultMonad;
using TagCloud.Settings;

namespace TagCloud.Visualizers
{
    public class CloudVisualizer : IVisualizer
    {
        private readonly IDrawingSettings drawingSettings;

        public CloudVisualizer(IDrawingSettings drawingSettings)
        {
            this.drawingSettings = drawingSettings;
        }

        public Result<Bitmap> DrawCloud(IEnumerable<Tag> tags,
            ITagColoringFactory tagColoringFactory)
        {
            if (drawingSettings.Width <= 0 || drawingSettings.Height <= 0)
                return Result.Fail<Bitmap>("Width and height should be positive, " +
                                           $"but was: Width = {drawingSettings.Width}, " +
                                           $"Height = {drawingSettings.Height}");
            var bitmap = new Bitmap(drawingSettings.Width, drawingSettings.Height);
            using var graph = Graphics.FromImage(bitmap);

            var penColors = ParseColors(drawingSettings.PenColors);
            if (!penColors.IsSuccess)
                return Result.Fail<Bitmap>(penColors.Error);

            var tagColoringAlgorithm = tagColoringFactory
                .Create(drawingSettings.AlgorithmName, penColors.Value);
            if (!tagColoringAlgorithm.IsSuccess)
                return Result.Fail<Bitmap>(tagColoringAlgorithm.Error);

            var backgroundBrush = Color.FromName(drawingSettings.BackgroundColor);
            if (!backgroundBrush.IsKnownColor)
                return Result.Fail<Bitmap>("Wrong background color name");

            graph.Clear(backgroundBrush);

            foreach (var tag in tags)
            {
                if (tag.Frequency == 0)
                    continue;
                
                if (!tag.ContainingRectangle.InsideSize(bitmap.Size))
                    return Result.Fail<Bitmap>("Tag is out of image");
                var color = tagColoringAlgorithm.Value.GetNextColor();
                using var brush = new SolidBrush(color);
                DrawTag(tag, graph, brush);
            }
            return bitmap.AsResult();
        }

        private void DrawTag(Tag tag, Graphics graph, Brush pen)
        {
            using var drawFont = new Font(drawingSettings.FontName, 
                drawingSettings.FontSize * tag.Frequency);
            graph.DrawString(tag.Value, drawFont, pen, tag.ContainingRectangle);
        }

        private static Result<IEnumerable<Color>> ParseColors(IEnumerable<string> colors)
        {
            var parsedColors = colors.Select(Color.FromName);
            return !parsedColors.Select(c => c.Name).SequenceEqual(colors)
                ? Result.Fail<IEnumerable<Color>>("Wrong color name") 
                : parsedColors.AsResult();
        }
    }
}
