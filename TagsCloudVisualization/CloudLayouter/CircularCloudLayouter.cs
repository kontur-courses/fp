using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudVisualization.Spirals;

namespace TagsCloudVisualization.CloudLayouter
{
    public class CircularCloudLayouter : ICloudLayouter
    {
        public List<Rectangle> Rectangles { get; }
        public ISpiral Spiral { get; }

        private const int NumberOfPoints = 10_000;

        public CircularCloudLayouter(ISpiral spiral)
        {
            Rectangles = new List<Rectangle>();
            Spiral = spiral;
        }

        public Result<Rectangle> PutNextRectangle(Size rectangleSize)
        {
            if (Spiral == null)
            {
                return Result.Fail<Rectangle>("Spiral cannot be null!");
            }

            if (rectangleSize.Height <= 0 || rectangleSize.Width <= 0)
                return Result.Fail<Rectangle>("Sides of the rectangle should not be non-positive");

            var points = Spiral.GetPoints(NumberOfPoints);
            var rectanglePosition = points[0];

            try
            {
                foreach (var point in points)
                {
                    if (!RectangleCanBePlaced(rectanglePosition, rectangleSize))
                        rectanglePosition = point;
                }
            }
            catch (Exception ex)
            {
                return Result.Fail<Rectangle>(ex.Message);
            }

            var rectangle = new Rectangle(rectanglePosition, rectangleSize);
            Rectangles.Add(rectangle);
            return rectangle;
        }

        private bool RectangleCanBePlaced(Point position, Size rectangleSize)
        {
            var rect = new Rectangle(position, rectangleSize);
            return !Rectangles.Any(rectangle => rectangle.IntersectsWith(rect));
        }
    }
}