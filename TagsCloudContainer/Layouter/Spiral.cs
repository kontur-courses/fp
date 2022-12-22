using System.Drawing;

namespace TagsCloudContainer.Layouter
{
    public class Spiral
    {
        private const double AngleStep = Math.PI / 10;
        private double _angle;
        private Point _prevPoint;

        public Spiral(Point start)
        {
            _prevPoint = start;
        }

        public Result<Point> NextPoint(int step)
        {
            if (step <= 0)
                return Result.Fail<Point>("Step must not be less than or equal to 0");

            _angle += AngleStep;
            var rho = _angle * step / (2 * Math.PI);
            var x = rho * Math.Cos(_angle) + _prevPoint.X;
            var y = rho * Math.Sin(_angle) + _prevPoint.Y;
            _prevPoint = new Point(Convert.ToInt32(x), Convert.ToInt32(y));
            return _prevPoint.Ok();
        }
    }
}