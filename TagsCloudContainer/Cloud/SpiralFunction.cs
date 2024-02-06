using System.Drawing;
using TagsCloudContainer.WordProcessing;

namespace TagsCloudContainer.Cloud
{
    public class SpiralFunction
    {
        private double _angle;
        private readonly Point _pastPoint;
        private readonly double _step;

        public SpiralFunction(Point start, double step)
        {
            _pastPoint = start;
            _step = step;
        }

        public Result<Point> GetNextPoint()
        {
            var newX = (int)(_pastPoint.X + _step * _angle * Math.Cos(_angle));
            var newY = (int)(_pastPoint.Y + _step * _angle * Math.Sin(_angle));
            _angle += Math.PI / 50;
            return new Point(newX, newY).Ok();
        }
    }
}