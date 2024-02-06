using ResultOf;
using System.Drawing;

namespace TagsCloudVisualization
{
    public interface IPointsProvider
    {
        public IEnumerable<Result<Point>> Points();
        public void Initialize(Point center);
    }
}