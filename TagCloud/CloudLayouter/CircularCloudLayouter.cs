using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagCloud.PointGenerator;
using TagCloud.ResultMonade;

namespace TagCloud.CloudLayouter
{
    public class CircularCloudLayouter : ICloudLayouter
    {
        public Point CloudCenter { get; }

        private readonly IPointGenerator pointGenerator;

        private readonly List<Rectangle> rectangles = new List<Rectangle>();

        public CircularCloudLayouter(IPointGenerator pointGenerator)
        {          
            this.pointGenerator = pointGenerator;

            CloudCenter = pointGenerator.CentralPoint;
        }

        public Result<Rectangle> PutNextRectangle(Size rectangleSize)
        {
            if (!IsValidRectangleSize(rectangleSize))
                return Result.Fail<Rectangle>("Width and height of rectangle must be more than zero");

            Rectangle rectangle;

            do
            {
                var pointToPutRectangle = pointGenerator.GetNextPoint();

                rectangle = new Rectangle(pointToPutRectangle, rectangleSize);

            } while (IsIntersectWithAnyExistingRectangle(rectangle));

            var shiftedRectangle = ShiftRectangleToCenterPoint(rectangle);

            rectangles.Add(shiftedRectangle);

            return shiftedRectangle;
        }

        public Size GetCloudSize()
        {
            var minTop = int.MaxValue;
            var maxBottom = -int.MaxValue;
            var minLeft = int.MaxValue;
            var maxRight = -int.MaxValue;

            foreach (var rectangle in rectangles)
            {
                if (rectangle.Top < minTop)
                    minTop = rectangle.Top;

                if (rectangle.Bottom > maxBottom)
                    maxBottom = rectangle.Bottom;

                if (rectangle.Left < minLeft)
                    minLeft = rectangle.Left;

                if (rectangle.Right > maxRight)
                    maxRight = rectangle.Right;
            }

            var width = 2 * Math.Max(Math.Abs(minLeft), Math.Abs(maxRight));

            var height = 2 * Math.Max(Math.Abs(maxBottom), Math.Abs(minTop));

            return new Size(width, height);
        }

        private static bool IsValidRectangleSize(Size rectangleSize)
        {
            return rectangleSize.Width > 0 && rectangleSize.Height > 0;
        }

        private bool IsIntersectWithAnyExistingRectangle(Rectangle rectangle)
        {
            return rectangles.Any(r => r.IntersectsWith(rectangle));
        }

        private Rectangle ShiftRectangleToCenterPoint(Rectangle rectangle)
        {
            var directionsToShift = GetDirectionsToShift(rectangle);

            var shiftedRectangleAlongX = ShiftRectangleAlongDirection(rectangle, directionsToShift.axisX);

            var shiftedRectangleAlongXAndY = ShiftRectangleAlongDirection(shiftedRectangleAlongX, directionsToShift.axisY);

            return shiftedRectangleAlongXAndY;
        }

        private (Vector axisX, Vector axisY) GetDirectionsToShift(Rectangle rectangle)
        {
            var deltaX = CloudCenter.X - rectangle.GetCenter().X > 0 ? 1 : -1;

            var deltaY = CloudCenter.Y - rectangle.GetCenter().Y > 0 ? 1 : -1;

            return (new Vector(deltaX, 0), new Vector(0, deltaY));
        }

        private Rectangle ShiftRectangleAlongDirection(Rectangle rectangle, Vector direction)
        {
            while (TryShiftRectangleAlongDirection(rectangle, direction, out var shiftedRectangle))
            {
                rectangle = shiftedRectangle;
            }

            return rectangle;
        }

        private bool TryShiftRectangleAlongDirection(Rectangle rectangle, Vector direction, out Rectangle shiftedRectangle)
        {
            if (IsRectangleAlignedAlongDirection(rectangle, direction))
            {
                shiftedRectangle = rectangle;

                return false;
            }

            shiftedRectangle = rectangle.MoveOn(direction.X, direction.Y);

            return !IsIntersectWithAnyExistingRectangle(shiftedRectangle);
        }

        private bool IsRectangleAlignedAlongDirection(Rectangle rectangle, Vector direction)
        {
            var vectorBetweenCenters = new Vector(CloudCenter, rectangle.GetCenter());

            return direction.IsPerpendicularTo(vectorBetweenCenters);
        }
    }
}
