using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudContainer.TagsCloudVisualization
{
    public class SpiralPointsGenerator
    {
        public Size Size { get; }

        private double currentAngle;
        private double spiralRadius;

        private readonly double angleDelta;
        private readonly double radiusDelta;
        private readonly Point center;

        public SpiralPointsGenerator(Point center, double startRadius = 10, double startAngle = 0,
            double angleDelta = Math.PI / 180, double radiusDelta = 0.01)
        {
            Size = new Size(2 * center.X, 2 * center.Y);
            currentAngle = startAngle;
            spiralRadius = startRadius;
            this.center = center;
            this.angleDelta = angleDelta;
            this.radiusDelta = radiusDelta;
        }

        public Result<IEnumerable<Point>> GetSpiralPoints()
        {
            if (spiralRadius < 0)
            {
                return Result.Fail<IEnumerable<Point>>("Radius can't be negative");
            }

            if (center.X < 0 || center.Y < 0)
            {
                return Result.Fail<IEnumerable<Point>>("Center coordinates can't be negative");
            }

            if (angleDelta == 0 || radiusDelta == 0)
            {
                return Result.Fail<IEnumerable<Point>>("Delta can't be zero");
            }

            return Result.Ok(GetPointSequence());
        }

        private IEnumerable<Point> GetPointSequence()
        {
            yield return center;
            while (true)
            {
                yield return new Point((int) (spiralRadius * Math.Cos(currentAngle) + center.X),
                    (int) (spiralRadius * Math.Sin(currentAngle) + center.Y));
                IncreaseParameters();
            }
        }

        private void IncreaseParameters()
        {
            spiralRadius += radiusDelta;
            currentAngle += angleDelta;
        }
    }
}