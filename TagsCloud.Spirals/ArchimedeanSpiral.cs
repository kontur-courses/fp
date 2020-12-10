using System;
using System.Drawing;
using TagsCloud.Common;
using TagsCloud.ResultPattern;

namespace TagsCloud.Spirals
{
    public class ArchimedeanSpiral : ISpiral
    {
        private readonly Point center;
        private readonly double spiralParameter;
        private double angle;
        private double radius;

        public ArchimedeanSpiral(Point center, double spiralParameter)
        {
            this.center = center;
            this.spiralParameter = spiralParameter;
            if (spiralParameter <= 0)
                Result.Fail<double>($"{nameof(spiralParameter)} must be a positive number").GetValueOrThrow();
        }

        public Point GetNextPoint()
        {
            var xLocation = (int)Math.Round(radius * Math.Cos(angle));
            var yLocation = (int)Math.Round(radius * Math.Sin(angle));

            radius += spiralParameter;
            angle += Math.PI / 180;

            return new Point(center.X + xLocation, center.Y + yLocation);
        }
    }
}