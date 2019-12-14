using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ErrorHandler;
using TagsCloudVisualization.Services;

namespace TagsCloudVisualization.Logic
{
    public class CircularCloudLayouter : ILayouter
    {
        private readonly ICirclePointLocator pointLocator;
        private List<Rectangle> taggedRectangles;

        public CircularCloudLayouter(ICirclePointLocator pointLocator)
        {
            taggedRectangles = new List<Rectangle>();
            this.pointLocator = pointLocator;
        }

        public Result<Rectangle> PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                return Result.Fail<Rectangle>("Image size can't be non-positive");
            return CreateRectangleOnSpiral(rectangleSize)
                .Then(rectangle =>
                {
                    taggedRectangles.Add(rectangle);
                    return rectangle;
                });
        }

        public void Reset()
        {
            taggedRectangles = new List<Rectangle>();
            pointLocator.DistanceFromCenter = 0;
            pointLocator.Angle = 0;
        }

        private Result<Rectangle> CreateRectangleOnSpiral(Size rectangleSize)
        {
            return Geometry.ShiftPointBySizeOffsets(Point.Empty, rectangleSize)
                .Then(shiftedCenter =>
                {
                    var rectangle = new Rectangle(shiftedCenter, rectangleSize);
                    while (taggedRectangles.Any(otherRectangle => rectangle.IntersectsWith(otherRectangle)))
                    {
                        var locatedPoint = pointLocator.GetNextPoint();
                        rectangle.X = shiftedCenter.X + locatedPoint.X;
                        rectangle.Y = shiftedCenter.Y + locatedPoint.Y;
                    }
                    return rectangle;
                })
                .Then(AlignLocatorDirection);
        }

        private Result<Rectangle> AlignLocatorDirection(Rectangle rectangle)
        {
            return Geometry.GetLengthFromRectangleCenterToBorderOnVector(rectangle, Point.Empty)
                .Then(distanceToBorder =>
                {
                    pointLocator.DistanceFromCenter -=
                        Math.Max(pointLocator.DistanceFromCenter / 2, distanceToBorder);
                    return rectangle;
                });
        }
    }
}