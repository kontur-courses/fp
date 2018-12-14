using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagCloud.TagCloudVisualization.Extensions;

namespace TagCloud.TagCloudVisualization.Layouter
{
    public class CircularCloudLayouter : ICloudLayouter
    {
        private readonly List<Rectangle> Rectangles;
        private IEnumerable<Point> spiralPoints;

        public CircularCloudLayouter()
        {
            Rectangles = new List<Rectangle>();
            spiralPoints = new Spiral().GenerateRectangleLocation();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var rectangle = GenerateNewRectangle(rectangleSize);
            Rectangles.Add(rectangle);
            return rectangle;
        }

        public void Clear()
        {
            Rectangles.Clear();
            spiralPoints = new Spiral().GenerateRectangleLocation();
        }

        private Rectangle GenerateNewRectangle(Size rectangleSize)
        {
            var rectangle = new Rectangle();
            foreach (var rectangleCenterPointLocation in spiralPoints)
            {
                var rectangleLocation = rectangleCenterPointLocation.ShiftToLeftRectangleCorner(rectangleSize);
                rectangle = new Rectangle(rectangleLocation, rectangleSize);
                if (RectanglesDoNotIntersect(rectangle))
                    break;
            }

            return rectangle;
        }

        private bool RectanglesDoNotIntersect(Rectangle newRectangle)
        {
            return !Rectangles.Any(newRectangle.IntersectsWith);
        }
    }
}