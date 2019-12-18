using System;
using System.Drawing;
using TagsCloud.Interfaces;

namespace TagsCloud.CloudLayouters
{
    public class RoundSpiralPositionGenerator : IPositionGenerator
    {
        private const double DeltaAngle = 0.5;
        private readonly double deltaRadiusBetweenTurns;
        private double angle;
        private Point center = new Point(0, 0);

        public RoundSpiralPositionGenerator(double deltaRadiusBetweenTurns = Math.PI)
        {
            this.deltaRadiusBetweenTurns = deltaRadiusBetweenTurns;
        }

        public Point GetNextPosition()
        {
            angle += DeltaAngle;
            var dist = deltaRadiusBetweenTurns * angle / 2 / Math.PI;
            var x = (int) (center.X + dist * Math.Cos(angle));
            var y = (int) (center.Y + dist * Math.Sin(angle));
            return new Point(x, y);
        }
    }
}