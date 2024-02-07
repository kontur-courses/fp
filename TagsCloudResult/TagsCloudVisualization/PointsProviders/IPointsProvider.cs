using System.Drawing;
using TagsCloudVisualization.Common.ResultOf;

namespace TagsCloudVisualization.PointsProviders;

public interface IPointsProvider
{
    public Point Start { get; }
    public Result<IEnumerable<Point>> GetPoints();
}
