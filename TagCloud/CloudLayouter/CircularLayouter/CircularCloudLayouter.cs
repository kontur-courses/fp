using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ErrorHandling;
using TagCloud.Visualization;

namespace TagCloud.CloudLayouter.CircularLayouter
{
    public class CircularCloudLayouter : ICloudLayouter
    {
        private readonly ArchimedeanSpiral spiral;
        public HashSet<Rectangle> Rectangles { get; set; }
        public ImageSettings LayouterSettings { get; }

        public CircularCloudLayouter(ArchimedeanSpiral spiral,
            ImageSettings layouterSettings)
        {
            LayouterSettings = layouterSettings;
            this.spiral = spiral;
            Rectangles = new HashSet<Rectangle>();
        }

        public void ResetLayouter()
        {
            spiral.SetNewStartPoint();
            Rectangles = new HashSet<Rectangle>();
        }

        public Result<Rectangle> PutNextRectangle(Size rectangleSize)
        {
            if (!IsCorrectSize(rectangleSize))
                return Result.Fail<Rectangle>(
                    $"Incorrect size of rectangle. Width: {rectangleSize.Width}, Height: {rectangleSize.Height}");

            foreach (var point in spiral.GetNewPointLazy())
            {
                var rect = new Rectangle(point, rectangleSize);
                if (!IsCorrectRectanglePosition(rect))
                    return Result.Fail<Rectangle>("Rectangle is out of layouter");
                if (!RectangleDoesNotIntersect(rect)) continue;
                Rectangles.Add(rect);
                return Result.Ok(rect);
            }

            return Result.Fail<Rectangle>("Failed to put next rectangle");
        }

        private bool IsCorrectSize(Size rectangleSize)
        {
            return !(rectangleSize.Width <= 0 || rectangleSize.Height <= 0);
        }

        private bool IsCorrectRectanglePosition(Rectangle rect)
        {
            return rect.Location.X + rect.Size.Width <= LayouterSettings.ImageSize.Width / 2 &&
                   rect.Location.X >= -LayouterSettings.ImageSize.Width / 2 &&
                   rect.Location.Y + rect.Size.Height <= LayouterSettings.ImageSize.Height / 2 &&
                   rect.Location.Y >= -LayouterSettings.ImageSize.Height / 2;
        }

        private bool RectangleDoesNotIntersect(Rectangle rectToAdd)
        {
            return Rectangles.All(rect => Rectangle.Intersect(rectToAdd, rect).IsEmpty);
        }
    }
}