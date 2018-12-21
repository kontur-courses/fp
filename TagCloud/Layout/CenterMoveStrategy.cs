using System;
using System.Drawing;

namespace TagCloud
{
    public class CenterMoveStrategy : IPlacementStrategy
    {
        public Rectangle PlaceRectangle(Rectangle newRectangle, Rectangle[] existingRectangles)
        {
            var stepX = -1 * Math.Sign(newRectangle.Location.X);
            var stepY = -1 * Math.Sign(newRectangle.Location.Y);

            var previousLocation = newRectangle.Location;
            while (!newRectangle.HasCollisionsWith(existingRectangles) && newRectangle.X != stepX)
            {
                previousLocation = newRectangle.Location;
                newRectangle = newRectangle.Move(stepX, 0);
            }
            newRectangle.Location = previousLocation;

            while (!newRectangle.HasCollisionsWith(existingRectangles) && newRectangle.Y != stepY)
            {
                previousLocation = newRectangle.Location;
                newRectangle = newRectangle.Move(0, stepY);
            }
            newRectangle.Location = previousLocation;

            return newRectangle;
        }
    }
}
