using System.Collections.Generic;
using System.Drawing;

namespace TagCloud.TagCloudVisualisation.Spirals
{
    public interface ISpiral
    {
        string Name { get; }
        IEnumerable<Point> GetPoints();
    }
}
