using ResultOf;
using System.Drawing;
using TagsCloudVisualization;

namespace TagsCloudContainer.TagCloudBuilder
{
    public class NormalPointsProvider : IPointsProvider
    {
        private readonly Random rnd = new();
        private Point Center;
        private int pointNumber = 0;
        private const int maxPointsCount = 10000000;

        public NormalPointsProvider(Point center)
        {
            Center = center;
        }
        public IEnumerable<Result<Point>> Points()
        {
            while (++pointNumber < maxPointsCount)
            {
                yield return Result<Point>.Ok(new Point((rnd.Next(0, Center.X) + rnd.Next(0, Center.X)), (rnd.Next(0, Center.Y) + rnd.Next(0, Center.Y))));
            }
            yield return Result<Point>.Fail("Can't get more points");
        }
    }
}