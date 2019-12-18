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
            var tagCloudRectangles = resultTagCloud as (Tag tag, Rectangle position)[] ?? resultTagCloud.ToArray();
            var borderOfRectangles = GetBorderOfRectangles(tagCloudRectangles);
            if (!borderOfRectangles.IsSuccess)
                return Result.Fail<Image>(borderOfRectangles.Error);
            using var image = new Bitmap(borderOfRectangles.Value.Width + widthOfBorder * 2,
                borderOfRectangles.Value.Height + widthOfBorder * 2);
            using var graph = Graphics.FromImage(image);
            graph.Clear(backgroundColor);
            foreach (var (tag, position) in tagCloudRectangles)
            {
                using var brush = new SolidBrush(tag.colorTag);
                using var font = new Font(tag.fontSettings.fontFamily, tag.fontSettings.fontSize);
                graph.DrawString(tag.word,
                    font, brush,
                    position.Left - borderOfRectangles.Value.X + widthOfBorder,
                    position.Top - borderOfRectangles.Value.Y + widthOfBorder);
            }
            return ((Image)new Bitmap(image, imageSize)).AsResult();
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
