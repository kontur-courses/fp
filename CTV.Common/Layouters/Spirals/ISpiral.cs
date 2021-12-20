using System.Collections.Generic;
using System.Drawing;

namespace CTV.Common.Layouters.Spirals
{
    public interface ISpiral
    {
        public IEnumerable<Point> GetEnumerator(Point center);
    }
}