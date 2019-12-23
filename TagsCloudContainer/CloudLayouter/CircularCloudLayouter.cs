using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CloudLayouter.Spiral;

namespace CloudLayouter
{
    public class CircularCloudLayouter : ICloudLayouter
    {
        protected readonly List<Rectangle> Rectangles;
        protected readonly ISpiral Spiral;

        public Point center;
        protected IEnumerator<Point> SpiralPoint;

        public CircularCloudLayouter(ISpiral spiral)
        {
            Rectangles = new List<Rectangle>();
            Spiral = spiral;
        }

        public void SetCenter(Point center)
        {
            this.center = center;
            SpiralPoint = Spiral.GetPoints(center).GetEnumerator();
        }


        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            while (true)
            {
                SpiralPoint.MoveNext();
                var point = SpiralPoint.Current;

                var rectangle =
                    new Rectangle(new Point(point.X - rectangleSize.Width / 2, point.Y - rectangleSize.Height / 2),
                        rectangleSize);
                if (HasOverlappingRectangles(rectangle, Rectangles)) continue;

                Rectangles.Add(rectangle);
                return rectangle;
            }
        }


        public static bool HasOverlappingRectangles(Rectangle rectangle, IEnumerable<Rectangle> rectangles)
        {
            return rectangles.Any(r => r.IntersectsWith(rectangle));
        }

        //различная реализация из-за сложностей алгоритмов 
        public static bool HasOverlappingRectangles(IEnumerable<Rectangle> rectangles)
        {
            return rectangles.Any(r => rectangles.Any(r1 => r != r1 && r.IntersectsWith(r1)));
        }
    }
}