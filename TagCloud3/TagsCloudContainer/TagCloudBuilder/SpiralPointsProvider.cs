using ResultOf;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class SpiralPointsProvider : IPointsProvider
    {
        private int pointNumber = 0;
        private readonly float angleStep = (float)Math.PI / 21;
        private readonly float SpiralRadius = 50;
        private Point Center;
        private const int maxPonitsCount = 10000000;

        public IEnumerable<Result<Point>> Points()
        {
            while (++pointNumber < maxPonitsCount)
            {
                var r = Math.Sqrt(SpiralRadius * pointNumber);
                var angle = angleStep * pointNumber;
                var x = r * Math.Cos(angle) + Center.X;
                var y = r * Math.Sin(angle) + Center.Y;
                yield return Result.Ok<Point>(new Point((int)x, (int)y));
            }
            yield return Result.Fail<Point>("Can't get more points");
        }

        public void Initialize(Point center)
        {
            Center = center;
        }
    }
}