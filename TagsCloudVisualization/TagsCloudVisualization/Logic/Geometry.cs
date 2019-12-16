using System;
using System.Drawing;
using ErrorHandler;

namespace TagsCloudVisualization.Logic
{
    public static class Geometry
    {
        public static Point PolarToCartesianCoordinates(double ro, double phi)
        {
            return new Point((int) (ro * Math.Cos(phi)), (int) (ro * Math.Sin(phi)));
        }

        public static Result<double> GetLengthFromRectangleCenterToBorderOnVector(Rectangle rect, Point vector)
        {
            if (rect.Width < 0 || rect.Height < 0)
                return Result.Fail<double>("Rectangle can't have negative sizes");

            double dx = Math.Abs(rect.X - vector.X);
            double dy = Math.Abs(rect.Y - vector.Y);
            var xOffset = rect.Width / 2;
            var yOffset = rect.Height / 2;
            if (dx <= xOffset && dy <= yOffset)
                return 0;
            if (dx > xOffset && dy > yOffset)
            {
                var k = xOffset / dx;
                dx = k * dx;
                dy = k * dy;
            }

            var hypotenuse = Math.Sqrt(dx * dx + dy * dy);
            var epsilon = 0.00001;
            if (dx - epsilon > xOffset)
                return hypotenuse * xOffset / dx;
            if (dy - epsilon > yOffset)
                return hypotenuse * yOffset / dy;
            return hypotenuse;
        }

        public static Result<Point> ShiftPointBySizeOffsets(Point point, Size size)
        {
            return size.Width < 0 || size.Height < 0
                ? Result.Fail<Point>("Offsets can't be negative")
                : new Point(point.X - size.Width / 2, point.Y - size.Height / 2);
        }
    }
}