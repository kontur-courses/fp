using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Functional;
using static Functional.Result;

namespace TagCloudVisualization
{
    public class CircularCloudLayouter
    {
        private readonly List<Rectangle> rectangles = new List<Rectangle>();

        private readonly AbstractSpiralGenerator spiralGenerator;

        public CircularCloudLayouter(Point center, AbstractSpiralGenerator generator)
        {
            Center = center;
            var result = generator.Begin(center);
            spiralGenerator = result.IsSuccess ? result.Value : throw new InvalidOperationException(result.Error);
        }

        public Point Center { get; }

        public IEnumerable<Rectangle> Rectangles => rectangles;

        public Result<Rectangle> PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                return Fail<Rectangle>("size has non positive parts");

            var nextPosition = spiralGenerator.Next();
            if (!nextPosition.IsSuccess)
                return Fail<Rectangle>(nextPosition.Error);

            var rectangle = new Rectangle(nextPosition.Value, rectangleSize);
            while (DoesIntersectWithPreviousRectangles(rectangle))
            {
                nextPosition = spiralGenerator.Next();
                if (!nextPosition.IsSuccess)
                    return Fail<Rectangle>(nextPosition.Error);
                rectangle = new Rectangle(nextPosition.Value, rectangleSize);
            }

            rectangle = AdjustPosition(rectangle);
            rectangles.Add(rectangle);

            return rectangle.AsResult();
        }

        private Rectangle AdjustPosition(Rectangle rectangle)
        {
            var oldRectangle = rectangle;
            var centerDirection = Center - rectangle.Location;
            var shiftX = Point.UnaryX * Math.Sign(centerDirection.X);
            var shiftY = Point.UnaryY * Math.Sign(centerDirection.Y);
            var stepsAmount = 100;
            while (stepsAmount > 0)
            {
                rectangle.Location += shiftX;
                if (DoesIntersectWithPreviousRectangles(rectangle))
                    rectangle.Location -= shiftX;

                rectangle.Location += shiftY;
                if (DoesIntersectWithPreviousRectangles(rectangle))
                    rectangle.Location += shiftY;

                stepsAmount--;
            }

            return DoesIntersectWithPreviousRectangles(rectangle) ? oldRectangle : rectangle;
        }

        private bool DoesIntersectWithPreviousRectangles(Rectangle rectangle) =>
            rectangles.Any(rectangle.IntersectsWith);

        public IEnumerable<Rectangle> PutNextRectangles(IEnumerable<Size> rectanglesSizes) =>
            rectanglesSizes.Select(PutNextRectangle).Select(w=>w.GetValueOrThrow());
    }
}
