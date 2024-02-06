using System.Drawing;

namespace TagsCloud.Distributors;

public interface IDistributor
{
    Result<Point> GetNextPosition();
}