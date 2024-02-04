using Results;
using System.Drawing;

namespace TagsCloudVisualization.PointCreators;

public interface IPointCreator
{
    public IEnumerable<Result<Point>> GetPoints();
}
