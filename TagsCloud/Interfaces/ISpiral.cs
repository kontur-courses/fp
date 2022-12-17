using System.Collections.Generic;
using System.Drawing;

namespace TagsCloud.Interfaces
{
    public interface ISpiral
    {
        public List<Point> Points { get; set; }
        public List<Point> FreePoints { get; set; }
        public Point Center { get; set; }

        public void AddPoint(Point pointToAdd);

        public void ReleasePoint(Point pointToRemove);

        public IEnumerable<Point> GetSpiralPoints();
    }
}