using System.Collections.Generic;
using System.Drawing;
using TagsCloudContainer.Extensions;
using TagsCloudContainer.Layouter.PointsProviders;

namespace TagsCloudContainer.Layouter
{
    public class CloudLayouter : ICloudLayouter
    {
        private readonly IPointsProvider pointsProvider;
        private readonly List<Rectangle> rectangles;

        public CloudLayouter(IPointsProvider pointsProvider)
        {
            this.pointsProvider = pointsProvider;
            rectangles = new List<Rectangle>();
        }

        public IReadOnlyCollection<Rectangle> Rectangles => rectangles;


        public Result<Rectangle> PutNextRectangle(Size size)
        {
            if (size.Height < 0 || size.Width < 0)
                return Result.Fail<Rectangle>("Negative size of rectangle is not expected");
            Rectangle nextRectangle;
            do
            {
                nextRectangle = new Rectangle(pointsProvider.GetNextPoint(), size);
            } while (nextRectangle.IntersectsWith(Rectangles));

            rectangles.Add(nextRectangle);
            return nextRectangle;
        }
    }
}