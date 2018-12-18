using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagCloud.RectanglesLayouter.PointsGenerator;

namespace TagCloud.RectanglesLayouter
{
    public class CircularCloudLayouter : IRectangleLayouter
    {
        private readonly Point center;
        private readonly IEnumerator<Point> pointsEnumerator;

        public CircularCloudLayouter(Point center, IPointsGenerator pointsGenerator)
        {
            this.center = center;
            pointsEnumerator = pointsGenerator.GetPoints().GetEnumerator();
        }

        public List<Rectangle> Rectangles { get; } = new List<Rectangle>();

        public Result<Rectangle> PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                return Result.Fail<Rectangle>("Rectangle width and height should be positive numbers");
            Rectangle newRectangle;
            do
            {
                pointsEnumerator.MoveNext();
                var nextPoint = pointsEnumerator.Current;
                var location = center + (Size)nextPoint;
                newRectangle = new Rectangle(location, rectangleSize).MoveToHavePointInCenter(location);
            } while (Rectangles.Any(rectangle => rectangle.IntersectsWith(newRectangle)));
            Rectangles.Add(newRectangle);
            return newRectangle;
        }
    }
}