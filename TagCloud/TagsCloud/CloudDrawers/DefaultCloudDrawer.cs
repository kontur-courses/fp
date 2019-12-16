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
            var borderOfRectangles = GetBorderOfRectangles(resultTagCloud);
            if (!borderOfRectangles.IsSuccess)
            {
                return Result.Fail<Image>(borderOfRectangles.Error);
            }
            var image = new Bitmap(borderOfRectangles.Value.Width + widthOfBorder * 2, borderOfRectangles.Value.Height + widthOfBorder * 2);
            using (var graph = Graphics.FromImage(image))
            {
                graph.Clear(backgroundColor);
                foreach (var tagSettings in resultTagCloud)
                {
                    using (var brush = new SolidBrush(tagSettings.tag.colorTag))
                    using (var font = new Font(tagSettings.tag.font.fontName, tagSettings.tag.font.fontSize))
                        graph.DrawString(tagSettings.tag.word,
                            font, brush, 
                            tagSettings.position.Left - borderOfRectangles.Value.X + widthOfBorder,
                            tagSettings.position.Top - borderOfRectangles.Value.Y + widthOfBorder);
                }
                image = new Bitmap(image, imageSize);
            }
            return ((Image)image).AsResult();
        }

        private static Result<Rectangle> GetBorderOfRectangles(IEnumerable<(Tag tag, Rectangle position)> rectangles)
        {
            if (rectangles.Count() == 0)
                return Result.Fail<Rectangle>("Cannot draw an empty collection.");
            var maxX = rectangles.Max(rectangle => rectangle.position.Right);
            var maxY = rectangles.Max(rectangle => rectangle.position.Bottom);
            var minX = rectangles.Min(rectangle => rectangle.position.Left);
            var minY = rectangles.Min(rectangle => rectangle.position.Top);
            return new Rectangle(minX, minY, maxX - minX, maxY - minY).AsResult();
        }
    }
}
