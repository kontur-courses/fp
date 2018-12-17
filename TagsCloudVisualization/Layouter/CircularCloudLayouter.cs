 using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ResultOf;

namespace TagsCloudVisualization.Layouter
{
    public class CircularCloudLayouter : ICloudLayouter
    {
        private readonly List<Rectangle> rectangles;

        private readonly Spiral spiral;

        public int Radius => GetSurroundingCircleRadius();

        private int GetSurroundingCircleRadius()
        {
            if (rectangles.Count == 0) return 0;
            return rectangles
                .Select(rect => new Point(MathHelper.MaxSignedAbs(rect.Left, rect.Right),
                    MathHelper.MaxSignedAbs(rect.Top, rect.Bottom)))
                .Select(point => point.GetDistanceTo(spiral.Center)).Max();
        }

        public CircularCloudLayouter(Spiral spiral)
        {
            this.spiral = spiral;
            rectangles = new List<Rectangle>();
        }

        public Result<Rectangle> PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Height <= 0 || rectangleSize.Width <= 0)
                return Result.Fail<Rectangle>("Size should be positive");
            var nextRectangle = GenerateNextRectangle(rectangleSize);
            rectangles.Add(nextRectangle);
            return Result.Ok(nextRectangle);
        }


        private Rectangle GenerateNextRectangle(Size rectangleSize)
        {
            while (true)
            {
                var rectangleCenter = spiral.GetNextPoint();
                var nexRectangle = new Rectangle(rectangleCenter, rectangleSize)
                    .ShiftRectangleToTopLeftCorner();
                if (!rectangles.Any(nexRectangle.IntersectsWith))
                    return nexRectangle;
            }
        }
    }
}
