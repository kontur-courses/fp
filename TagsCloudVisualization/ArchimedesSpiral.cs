using System;
using System.Drawing;
using ResultOfTask;

namespace TagsCloudVisualization
{
    public class ArchimedesSpiral : ISpiral
    {
        private const double SpiralShift = 0.5;
        private const double AngleShift = 0.01;
        private double angle;

        public ArchimedesSpiral(Point center)
        {
            Center = center;
        }

        public Point Center { get; }

        public Result<Rectangle> GetRectangleInNextLocation(Size rectangleSize)
        {
            angle += AngleShift;
            var rectangle = new Rectangle(GetCurrentPositionOnTheSpiral(), rectangleSize);

            return Result.Ok(rectangle.ShiftCoordinatesToCenterRectangle());
        }

        private Point GetCurrentPositionOnTheSpiral()
        {
            var x = Center.X + SpiralShift * angle * Math.Cos(angle);
            var y = Center.Y + SpiralShift * angle * Math.Sin(angle);

            return new Point((int) x, (int) y);
        }
    }
}