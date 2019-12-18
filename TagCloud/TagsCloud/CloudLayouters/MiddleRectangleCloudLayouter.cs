using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloud.ErrorHandling;
using TagsCloud.Extensions;
using TagsCloud.Interfaces;

namespace TagsCloud.CloudLayouters
{
    public class MiddleRectangleCloudLayouter : ITagCloudLayouter
    {
        private readonly Point center;
        private readonly SortedList<double, List<(Point point, int countNear)>> pointForAdd;
        private readonly List<Rectangle> previousRectangles;

        public MiddleRectangleCloudLayouter()
        {
            center = new Point(0, 0);
            previousRectangles = new List<Rectangle>();
            pointForAdd = new SortedList<double, List<(Point point, int countNear)>>
            {
                {0, new List<(Point point, int countNear)> {(new Point(0, 0), 0)}}
            };
        }

        public Result<Rectangle> PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width < 0 || rectangleSize.Height < 0)
                return Result.Fail<Rectangle>("The rectangle cannot have a negative length side");
            var rect = new Rectangle(0, 0, rectangleSize.Width, rectangleSize.Height);
            var deltaRectangleCorner = GetCornerPositions(rect).ToList();
            var keys = pointForAdd.Keys;
            for (var j = 0; j < keys.Count; j++)
            {
                var distanceToCenter = keys[j];
                for (var i = 0; i < pointForAdd[distanceToCenter].Count; i++)
                {
                    var (point, countNear) = pointForAdd[distanceToCenter][i];
                    foreach (var deltaCorner in deltaRectangleCorner)
                    {
                        rect.MoveToPosition(new Point(point.X - deltaCorner.X,
                            point.Y - deltaCorner.Y));
                        if (IntersectsWithPrevious(rect)) continue;
                        AddRectangle(rect);
                        countNear += 1;
                        if (countNear != 2) return rect;
                        pointForAdd[distanceToCenter].RemoveAt(i);
                        if (pointForAdd[distanceToCenter].Count == 0)
                            pointForAdd.RemoveAt(j);
                        return rect;
                    }
                }
            }

            return rect.AsResult();
        }

        private void AddRectangle(Rectangle rect)
        {
            foreach (var middles in GetMiddlesSidesRectangle(rect))
            {
                var point = new Point(rect.X + middles.X, rect.Y + middles.Y);
                var distanceToCenter = point.GetDistance(center);
                if (!pointForAdd.ContainsKey(distanceToCenter))
                    pointForAdd.Add(distanceToCenter, new List<(Point point, int countNear)>());
                pointForAdd[distanceToCenter].Add((point, 0));
            }

            previousRectangles.Add(rect);
        }

        private static IEnumerable<Point> GetCornerPositions(Rectangle rectangle)
        {
            yield return new Point(0, 0);
            yield return new Point(rectangle.Width, 0);
            yield return new Point(rectangle.Width, rectangle.Height);
            yield return new Point(0, rectangle.Height);
        }

        private static IEnumerable<Point> GetMiddlesSidesRectangle(Rectangle rectangle)
        {
            yield return new Point(rectangle.Width / 2, 0);
            yield return new Point(rectangle.Width, rectangle.Height / 2);
            yield return new Point(rectangle.Width / 2, rectangle.Height);
            yield return new Point(0, rectangle.Height / 2);
        }

        private bool IntersectsWithPrevious(Rectangle rectangle)
        {
            return previousRectangles.Any(previousRectangle => previousRectangle.IntersectsWith(rectangle));
        }
    }
}