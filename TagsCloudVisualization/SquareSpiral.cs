using System;
using System.Drawing;
using ResultOfTask;

namespace TagsCloudVisualization
{
    public class SquareSpiral : ISpiral
    {
        private const double SpiralShift = 1;
        private const double AngleShift = 0.05;

        private static readonly int l = 10;
        private static readonly int t = 10;

        private static readonly int y0 = t;
        private static readonly int y1 = y0;
        private static readonly int y2 = y0 + l + t;
        private static readonly int y3 = y0 + l + t;

        private static readonly int x0 = l;
        private static readonly int x1 = x0 + l;
        private static readonly int x2 = x0 + l;
        private static readonly int x3 = x0 - l;

        private int num;

        public SquareSpiral(Point center)
        {
            Center = center;
        }

        public Point Center { get; }

        public Result<Rectangle> GetRectangleInNextLocation(Size rectangleSize)
        {
            var resultOfPosition = GetCurrentPositionOnTheSpiral();
            return resultOfPosition.IsSuccess 
                ? Result.Of(() => 
                    new Rectangle(resultOfPosition.GetValueOrThrow(), rectangleSize)
                        .ShiftCoordinatesToCenterRectangle()) 
                : Result.Fail<Rectangle>(resultOfPosition.Error);
        }

        private Result<Point> GetCurrentPositionOnTheSpiral()
        {
            num++;
            var point = Center;
            switch (num - num % 4)
            {
                case 0:
                    point.Offset(new Point(x0 - t * num, y0 - t * num));
                    break;
                case 1:
                    point.Offset(new Point(x1 + t * num, y1 - t * num));
                    break;
                case 2:
                    point.Offset(new Point(x2 + t * num, y2 + t * num));
                    break;
                case 3:
                    point.Offset(new Point(x3 - t * num, y3 + t * num));
                    break;
            }

            return Result.Fail<Point>("Not implemented");
        }
    }
}