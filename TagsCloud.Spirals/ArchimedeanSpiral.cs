using System;
using System.Drawing;
using TagsCloud.Common;

namespace TagsCloud.Spirals
{
    public class ArchimedeanSpiral : ISpiral
    {
        private readonly Point center;
        private readonly SpiralSettings spiralSettings;
        private double angle;
        private double radius;

        public ArchimedeanSpiral(Point center, SpiralSettings spiralSettings)
        {
            this.center = center;
            this.spiralSettings = spiralSettings;
        }

        public Point GetNextPoint()
        {
            var xLocation = (int)Math.Round(radius * Math.Cos(angle));
            var yLocation = (int)Math.Round(radius * Math.Sin(angle));

            radius += spiralSettings.SpiralParameter;
            angle += Math.PI / 180;

            return new Point(center.X + xLocation, center.Y + yLocation);
        }
    }
}