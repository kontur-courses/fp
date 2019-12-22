using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloud.CloudConstruction.Exceptions;
using TagsCloud.CloudConstruction.Extensions;
using TagsCloud.ErrorHandler;

namespace TagsCloud.CloudConstruction
{
    public class CircularCloudLayouter : ICloudLayouter
    {
        public readonly Point Center;
        private const int Frequency = 36;

        public List<Rectangle> Rectangles { get; set; }

        public CircularCloudLayouter(Point center)
        {
            this.Center = center;
            Rectangles = new List<Rectangle>();
        }

        public Result<Rectangle> PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                return Result.Fail<Rectangle>($"Wrong rectangle size: {rectangleSize}. Width or height can't be negative");
            var rectangle = GenerateRectangle(rectangleSize);
            if (!rectangle.IsSuccess)
            {
                return Result.Fail<Rectangle>(rectangle.Error);
            }
            Rectangles.Add(rectangle.Value);
            return rectangle;
        }

        private Result<Rectangle> GenerateRectangle(Size rectSize)
        {
            try
            {
                var center = SpiralPointGenerator()
                    .First(centerPoint =>
                        !NewRectangleIntersectsWithAny(centerPoint, rectSize));
                return new Rectangle(ConvertToLocation(center, rectSize), rectSize);
            }
            catch (InvalidOperationException)
            {
                return Result.Fail<Rectangle>("Can't find correct location");
            }
        }

        private bool NewRectangleIntersectsWithAny(Point rectCenter, Size rectSize)
        {
            var rect = new Rectangle(ConvertToLocation(rectCenter, rectSize), rectSize);
            return rect.IntersectsWithAny(Rectangles);
        }

        private static Point ConvertToLocation(Point center, Size recSize)
        {
            return new Point(center.X - recSize.Width / 2, center.Y - recSize.Height / 2);
        }

        private IEnumerable<Point> SpiralPointGenerator()
        {
            var angle = 0.0;
            var curPoint = Center;
            while (true)
            {
                yield return curPoint;
                angle += Math.PI / Frequency;

                var x = (int) (angle * Math.Cos(angle) - Center.X);
                var y = (int) (angle * Math.Sin(angle) - Center.Y);
                curPoint = new Point(x, y);
            }
        }
    }
}