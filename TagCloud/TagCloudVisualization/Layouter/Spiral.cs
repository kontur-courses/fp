using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagCloud.TagCloudVisualization.Layouter
{
    public class Spiral
    {
        private const double AngleShift = 0.05;
        private const int DistanceBetweenTurnings = 1;
        private double currentSpiralAngle;

        private void IncreaseSpiralAngle()
        {
            currentSpiralAngle += AngleShift;
        }

        public IEnumerable<Point> GenerateRectangleLocation()
        {
            while (true)
            {
                var radius = DistanceBetweenTurnings * currentSpiralAngle;
                var x = (int) (radius * Math.Cos(currentSpiralAngle));
                var y = (int) (radius * Math.Sin(currentSpiralAngle));
                yield return new Point(x, y);
                IncreaseSpiralAngle();
            }
        }
    }
}