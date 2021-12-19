using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using App.Implementation.GeometryUtils;
using App.Infrastructure.LayoutingAlgorithms.AlgorithmFromTDD;

namespace App.Implementation.LayoutingAlgorithms.AlgorithmFromTDD
{
    public class CircularLayouter : ICloudLayouter
    {
        private readonly DirectingArrow arrow;
        private readonly List<Rectangle> rectangles;

        public CircularLayouter(Point center = default)
        {
            Center = center;
            rectangles = new List<Rectangle>();
            arrow = new DirectingArrow(Center);
        }

        public Point Center { get; }

        public List<Rectangle> GetRectanglesCopy()
        {
            return new List<Rectangle>(rectangles);
        }

        public int GetCloudBoundaryRadius()
        {
            return rectangles.Count == 0
                ? 0
                : (int)Math.Ceiling(rectangles
                    .SelectMany(rect => rect
                        .GetCorners())
                    .Max(corner => corner.GetDistanceTo(Center)));
        }

        public Size GetRectanglesBoundaryBox()
        {
            if (rectangles.Count == 0)
                return Size.Empty;

            var width
                = rectangles.Max(rect => rect.Right) - rectangles.Min(rect => rect.X);
            var height
                = rectangles.Max(rect => rect.Bottom) - rectangles.Min(rect => rect.Y);

            return new Size(width, height);
        }

        public Result<Rectangle> PutNextRectangle(Size rectangleSize)
        {
            var rectangleResult = CheckForIncorrectSize(rectangleSize);

            if (!rectangleResult.IsSuccess)
                return rectangleResult;

            var rectangle = SetRectangleToCenter(rectangleSize);

            while (rectangle.IntersectsWithAny(rectangles))
                rectangle = RotateRectangle(rectangle);

            rectangles.Add(rectangle);
            return Result.Ok(rectangle);
        }

        private Rectangle RotateRectangle(Rectangle rectangle)
        {
            return new Rectangle(
                arrow.Rotate().MovePointToSizeCenter(rectangle.Size, false),
                rectangle.Size);
        }

        private Rectangle SetRectangleToCenter(Size rectangleSize)
        {
            return new Rectangle(
                Center.MovePointToSizeCenter(rectangleSize, false),
                rectangleSize);
        }

        private static Result<Rectangle> CheckForIncorrectSize(Size rectangleSize)
        {
            return rectangleSize.Width <= 0 || rectangleSize.Height <= 0
                ? Result.Fail<Rectangle>("Width and height of rectangle must be a positive numbers")
                : Result.Ok(new Rectangle(Point.Empty, rectangleSize));
        }
    }
}