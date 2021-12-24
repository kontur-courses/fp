﻿using System;
using System.Drawing;

namespace TagsCloud.Visualization.PointGenerator
{
    public class ArchimedesSpiralPointGenerator : IPointGenerator
    {
        private readonly double angleDelta;
        private readonly double xAxisCompression;
        private readonly double yAxisCompression;
        private double angle;

        public ArchimedesSpiralPointGenerator(
            Point center,
            double angleDelta = Math.PI / 360,
            double xAxisCompression = 1,
            double yAxisCompression = 1)
        {
            Center = center;
            this.angleDelta = angleDelta;
            this.xAxisCompression = xAxisCompression;
            this.yAxisCompression = yAxisCompression;
        }

        public Point Center { get; }

        public Point GenerateNextPoint()
        {
            var x = Convert.ToInt32(xAxisCompression * Math.Cos(angle) * angle + Center.X);
            var y = Convert.ToInt32(yAxisCompression * Math.Sin(angle) * angle + Center.Y);

            angle += angleDelta;

            return new Point(x, y);
        }
    }
}