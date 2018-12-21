using System;
using System.Drawing;

namespace TagCloud
{
    public class SpiralStrategy : IPlacementStrategy
    {
        public Rectangle PlaceRectangle(Rectangle newRectangle, Rectangle[] existingRectangles)
        {
            for (var step = 1; newRectangle.HasCollisionsWith(existingRectangles); step++)
            {
                var x = step * Math.Cos(step);
                var y = step * Math.Sin(step);
                newRectangle.Location = new Point((int)x, (int)y);
            }

            return newRectangle;
        }
    }
}
