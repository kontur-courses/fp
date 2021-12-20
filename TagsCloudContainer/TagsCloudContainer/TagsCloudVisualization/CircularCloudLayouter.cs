using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudContainer.TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public readonly Size Size;
        public readonly List<Rectangle> Rectangles;
        private readonly SpiralPointsGenerator pointsGenerator;

        public CircularCloudLayouter(SpiralPointsGenerator pointsGenerator)
        {
            Size = pointsGenerator.Size;
            Rectangles = new List<Rectangle>();
            this.pointsGenerator = pointsGenerator;
        }

        public Result<Rectangle> PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
            {
                return Result.Fail<Rectangle>("Width and height can't be negative");
            }

            return GetNextPointForRectangle(rectangleSize)
                .Then(location => new Rectangle(location, rectangleSize))
                .ThenDo(rectangle => Rectangles.Add(rectangle));
        }

        private Result<Point> GetNextPointForRectangle(Size rectangleSize)
        {
            var point = pointsGenerator.GetSpiralPoints()
                .Then(points =>
                    points.FirstOrDefault(p => !new Rectangle(p, rectangleSize).IntersectsWithRectangles(Rectangles)));
            if (point.IsSuccess && (point.Value.X < 0 || point.Value.Y < 0 ||
                                    point.Value.X + rectangleSize.Width > Size.Width ||
                                    point.Value.Y + rectangleSize.Height > Size.Height))
            {
                return Result.Fail<Point>("Don't have enough space to put next rectangle");
            }

            return point;
        }
    }
}