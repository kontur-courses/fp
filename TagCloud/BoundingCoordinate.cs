using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloud
{
    public class BoundingCoordinate
    {
        private readonly IReadOnlyCollection<Rectangle> allRectangles;

        public BoundingCoordinate(IReadOnlyCollection<Rectangle> allRectangles)
        {
            this.allRectangles = allRectangles;
        }

        public int MaxX => allRectangles.Max(rectangle => rectangle.X + rectangle.Width);
        public int MaxY => allRectangles.Max(rectangle => rectangle.Y + rectangle.Height);
        public int MinX => allRectangles.Min(rectangle => rectangle.X);
        public int MinY => allRectangles.Min(rectangle => rectangle.Y);

        public int SizeX => MaxX - MinX;
        public int SizeY => MaxY - MinY;
    }
}