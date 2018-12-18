using System.Collections.Generic;
using System.Drawing;
using TagsCloudContainer.Extensions;
using TagsCloudContainer.WordsFilters;

namespace TagsCloudContainer.CircularCloudLayouters
{
    public class CircularCloudLayouter : ICircularCloudLayouter
    {
        private readonly IEnumerator<Point> pointsOrder;
        private readonly List<Rectangle> addedRectangles = new List<Rectangle>();
        private readonly IFilter<Rectangle> rectanglesFilter;
        private const int MaxDirectionChoosingIterationsForOneRectangle = 2000;

        public CircularCloudLayouter(IEnumerator<Point> pointsOrder, IFilter<Rectangle> rectanglesFilter)
        {
            this.pointsOrder = pointsOrder;
            this.rectanglesFilter = rectanglesFilter;
        }

        public Result<Rectangle> PutNextRectangle(Size size)
        {
            pointsOrder.MoveNext();
            return GetTheNearestFreeRectangle(size)
                .Then(AddRectangle);
        }

        private Rectangle AddRectangle(Rectangle rectangle)
        {
            addedRectangles.Add(rectangle);
            return rectangle;
        }

        private Result<Rectangle> GetTheNearestFreeRectangle(Size size)
        {
            for (var index = 0; index < MaxDirectionChoosingIterationsForOneRectangle; index++)
            {
                var nextRectangle = GetRectangleAtCurrentPoint(size);
                if (rectanglesFilter.IsCorrect(nextRectangle) &&
                    !nextRectangle.IntersectsWithPreviousRectangles(addedRectangles))
                    return nextRectangle;
                pointsOrder.MoveNext();
            }
            return Result.Fail<Rectangle>("Can not allocate all words.");
        }

        private Rectangle GetRectangleAtCurrentPoint(Size size)
        {
            var centerPoint = pointsOrder.Current;
            var leftTopPoint = centerPoint.Shift(-size.Width / 2, -size.Height / 2);
            return new Rectangle(leftTopPoint, size);
        }
    }
}