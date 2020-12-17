using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudVisualization.AppSettings;
using TagsCloudVisualization.Canvases;
using TagsCloudVisualization.Tags;

namespace TagsCloudVisualization.TagCloudVisualizers
{
    public class TagCloudVisualizer : ICloudVisualizer
    {
        private readonly PaintingSettings paintingSettings;
        private readonly ICanvas canvas;

        public TagCloudVisualizer(ICanvas canvas, PaintingSettings paintingSettings)
        {
            this.canvas = canvas;
            this.paintingSettings = paintingSettings;
        }

        public Result<None> PrintTagCloud(IEnumerable<Tag> tags)
        {
            if (tags == null)
                return Result.Fail<None>("Instead of the expected tag cloud for rendering, came 'null'");
            
            if (!CheckTagCloudSize(tags))
                return Result.Fail<None>("Tag cloud could not fit to image size");
            
            var imageSize = canvas.GetImageSize();
            
            using (var graphics = canvas.StartDrawing())
            {
                using (var backgroundBrush = new SolidBrush(paintingSettings.BackgroundColor))
                    graphics.FillRectangle(backgroundBrush, 0, 0, imageSize.Width, imageSize.Height);

                using (var textBrush = new SolidBrush(paintingSettings.TextColor)) 
                    DrawCloud(tags, textBrush, graphics);
            }
            
            canvas.UpdateUi();
            return Result.Ok();
        }

        private bool CheckTagCloudSize(IEnumerable<Tag> tags)
        {
            var tagsArray = tags.ToArray();
            var maxX = tagsArray.Max(tag => tag.BoundingZone.X);
            var maxY = tagsArray.Max(tag => tag.BoundingZone.Y);
            var minX = tagsArray.Min(tag => tag.BoundingZone.X);
            var minY = tagsArray.Min(tag => tag.BoundingZone.Y);

            var imageSize = canvas.GetImageSize();
            return maxX <= imageSize.Width && maxY <= imageSize.Height && minX >= 0 && minY >= 0;
        }

        private void DrawCloud(IEnumerable<Tag> tags, SolidBrush textBrush, Graphics graphics)
        {
            var pen = new Pen(Color.White);
            var iterationCount = 0;
            foreach (var tag in tags)
            {
                if (paintingSettings.EnabledMultipleColors)
                    textBrush.Color = paintingSettings.Colors[iterationCount % paintingSettings.Colors.Length];
                
                graphics.DrawString(tag.Text, tag.Font, textBrush, tag.BoundingZone.Location);

                if (paintingSettings.DrawTextBoundingZone)
                {
                    pen.Color = Color.Beige;
                    graphics.DrawRectangle(pen, tag.BoundingZone);
                }

                iterationCount++;
                if (paintingSettings.EnableAnimation)
                    canvas.UpdateUi();
            }
        }
    }
}