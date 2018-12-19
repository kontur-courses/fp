using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudContainer.CloudLayouters
{
    public class CircularCloudLayouter : ICloudLayouter
    {
        private readonly List<Rectangle> cloudRectangles = new List<Rectangle>();
        private readonly IEnumerable<Point> generatorPoints;

        public CircularCloudLayouter(IEnumerable<Point> generatorPoints)
        {
            this.generatorPoints = generatorPoints;
        }

        public Result<Rectangle> PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Height < 0 || rectangleSize.Width < 0)
                return Result.Fail<Rectangle>("height and width should be positive");

            var currentRectangle = new Rectangle();
            foreach (var point in generatorPoints)
            {
                var rectangleCenterPoint = new Point(
                    point.X - rectangleSize.Width / 2,
                    point.Y - rectangleSize.Height / 2);

                currentRectangle = new Rectangle(rectangleCenterPoint, rectangleSize);
                if (!cloudRectangles.Any(rect => rect.IntersectsWith(currentRectangle)))
                {
                    cloudRectangles.Add(currentRectangle);
                    break;
                }
            }

            return Result.Ok(currentRectangle);
        }
    }
}