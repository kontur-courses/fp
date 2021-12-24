using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagCloud.layouter
{
    public class CircularCloudLayouter : ICloudLayouter
    {
        private readonly List<Rectangle> rectangles;
        private readonly SortedDistinctSingleLinkedList<Point> points;

        private readonly IEnumerable<Func<Point, Size, Point>> shifts = new List<Func<Point, Size, Point>>
        {
            (p, s) => p,
            (p, s) => new Point(p.X, p.Y - s.Height),
            (p, s) => new Point(p.X - s.Width, p.Y - s.Height),
            (p, s) => new Point(p.X - s.Width, p.Y)
        };

        public CircularCloudLayouter()
        {
            var center = Point.Empty;
            rectangles = new List<Rectangle>();
            points = new SortedDistinctSingleLinkedList<Point>(
                (p1, p2) => center.DistanceTo(p1) < center.DistanceTo(p2)
            );
            points.Add(center);
        }

        public Result<RectangleF> PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Height <= 0 || rectangleSize.Width <= 0)
                return Result.Fail<RectangleF>(ResultErrorType.RectangleSizeError);

            var rectangle = Rectangle.Empty;
            foreach (var point in points.ToEnumerable())
            {
                var putResult = PutRectangleToCorners(point, rectangleSize);
                if (!putResult.IsSuccess)
                    continue;
                rectangle = putResult.Value;
                break;
            }

            rectangles.Add(rectangle);
            return SaveRectangleBorderPoints(rectangle);
        }

        private Result<RectangleF> SaveRectangleBorderPoints(Rectangle rectangle)
        {
            points.Add(new Point(rectangle.Right, rectangle.Bottom));
            points.Add(new Point(rectangle.Left, rectangle.Bottom));
            points.Add(new Point(rectangle.Right, rectangle.Top));
            points.Add(new Point(rectangle.Left, rectangle.Top));
            return Result.Ok((RectangleF)rectangle);
        }

        private Result<Rectangle> PutRectangleToCorners(Point point, Size rectangleSize)
        {
            foreach (var shift in shifts)
            {
                var location = shift(point, rectangleSize);
                var rectangle = new Rectangle(location, rectangleSize);
                if (!rectangles.IntersectsWith(rectangle))
                    return Result.Ok(rectangle);
            }

            return Result.Fail<Rectangle>(ResultErrorType.RectanglePutError);
        }
    }
}