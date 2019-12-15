using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CSharpFunctionalExtensions;

namespace TagsCloudLibrary.Layouters
{
    public class CircularCloudLayouter : ILayouter
    {
        public string Name { get; } = "Circular";

        private Point _center;
        private readonly List<Rectangle> _placedRectangles = new List<Rectangle>();

        public CircularCloudLayouter()
        {
            _center = Point.Empty;
        }

        public CircularCloudLayouter(Point center)
        {
            _center = center;
        }

        public Result<Rectangle> PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                return Result.Failure<Rectangle>("rectangleSize is not correct rectangle size");

            var (x, y) = GetNextUpperLeftCornerPosition(rectangleSize);
            var newRectangle = new Rectangle(x, y, rectangleSize.Width, rectangleSize.Height);
            while (_placedRectangles.Any(pr => pr.IntersectsWith(newRectangle)))
                (newRectangle.X, newRectangle.Y) = GetNextUpperLeftCornerPosition(newRectangle.Size);

            _placedRectangles.Add(newRectangle);
            return Result.Ok(newRectangle);
        }

        private double _spiralPosition;
        private const double SpiralFactor = 0.5;
        private const double Step = 1 / 50d;

        private (int x, int y) GetNextUpperLeftCornerPosition(Size size)
        {
            var x = (int)(_center.X + SpiralFactor * _spiralPosition * Math.Cos(_spiralPosition) - size.Width / 2d);
            var y = (int)(_center.Y + SpiralFactor * _spiralPosition * Math.Sin(_spiralPosition) - size.Height / 2d);
            _spiralPosition += Step;
            return (x, y);
        }
    }
}
