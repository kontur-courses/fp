using ResultOf;
using System.Drawing;
using TagsCloudVisualization;

namespace TagsCloudContainer.TagCloudBuilder
{
    public class RandomPointsProvider : IPointsProvider
    {
        private Random rnd = new Random();
        private Point Center;
        private int pointNumber = 0;
        private const int maxPonitsCount = 10000000;

        public IEnumerable<Result<Point>> Points()
        {
            while (++pointNumber < maxPonitsCount)
            {
                yield return Result<Point>.Ok(new Point(rnd.Next(0, Center.X * 2), rnd.Next(0, Center.Y * 2)));
            }
            yield return Result<Point>.Fail("Can't get more points");
        }

        public void Initialize(Point center)
        {
            Center = center;
        }
    }
}