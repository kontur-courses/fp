using System;
using System.Drawing;

namespace TagsCloudContainer
{
    public static class PointExtensions
    {
        public static double DistanceTo(this Point first, PointF second)
        {
            return Math.Sqrt((first.X - second.X) * (first.X - second.X) + (first.Y - second.Y) * (first.Y - second.Y));
        }
    }
}