using System.Drawing;

namespace TagsCloudResult.Algorithms
{
    public interface ISpiral
    {
        Result<Point> GetNextPoint();
    }
}