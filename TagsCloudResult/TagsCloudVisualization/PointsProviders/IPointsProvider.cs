using System.Drawing;
using TagsCloudVisualization.Common;

namespace TagsCloudVisualization.PointsProviders;

public interface IPointsProvider
{
    public Point Start { get; }
    public Result<IEnumerable<Point>> GetPoints();
}