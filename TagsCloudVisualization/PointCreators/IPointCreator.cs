using Results;
using System.Drawing;

namespace TagsCloudVisualization.PointCreators;

public interface IPointCreator
{
    public Point GetNextPoint();
    public Result<bool> CheckForCorrectness();
}