using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloud.Interfaces;
using TagsCloud.TagGenerators;
using TagsCloud.ErrorHandling;

namespace TagsCloud.CloudDrawers
{
    public class DefaultCloudDrawer : ICloudDrawer
    {
        public Result<Image> Paint(IEnumerable<(Tag tag, Rectangle position)> resultTagCloud, Size imageSize, Color backgroundColor, int widthOfBorder = 0)
        {
            return GetBorderOfRectangles(resultTagCloud)
                .Then(borderOfRectangles => (border:borderOfRectangles, 
                    image: new Bitmap(borderOfRectangles.Width + widthOfBorder * 2, borderOfRectangles.Height + widthOfBorder * 2)))
                .Then(drawSettings =>
                {
                    var image = drawSettings.image;
                    var borderOfRectangles = drawSettings.border;
                    using (var graph = Graphics.FromImage(image))
                    {
                        graph.Clear(backgroundColor);
                        foreach (var tagSettings in resultTagCloud)
                        {
                            using (var brush = new SolidBrush(tagSettings.tag.colorTag))
                            using (var font = new Font(tagSettings.tag.font.fontName, tagSettings.tag.font.fontSize))
                                graph.DrawString(tagSettings.tag.word,
                                    font, brush,
                                    tagSettings.position.Left - borderOfRectangles.X + widthOfBorder,
                                    tagSettings.position.Top - borderOfRectangles.Y + widthOfBorder);
                        }
                        return new Bitmap(image, imageSize);
                    }
                })
                .Then(bitmapImage => (Image)bitmapImage);
        }

        private static Result<Rectangle> GetBorderOfRectangles(IEnumerable<(Tag tag, Rectangle position)> rectangles)
        {
            if (!rectangles.Any())
                return Result.Fail<Rectangle>("Cannot draw an empty collection.");
            var maxX = rectangles.Max(rectangle => rectangle.position.Right);
            var maxY = rectangles.Max(rectangle => rectangle.position.Bottom);
            var minX = rectangles.Min(rectangle => rectangle.position.Left);
            var minY = rectangles.Min(rectangle => rectangle.position.Top);
            return new Rectangle(minX, minY, maxX - minX, maxY - minY).AsResult();
        }
    }
}
