using System.Drawing;

namespace TagsCloud.Visualization.PointGenerator
{
    public interface IPointGenerator
    {
        Point Center { get; }
        Point GenerateNextPoint();
    }
}