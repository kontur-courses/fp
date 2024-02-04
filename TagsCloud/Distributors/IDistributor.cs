using System.Drawing;
using TagsCloud.Result;

namespace TagsCloud.Distributors;

public interface IDistributor
{
    Result<Point> GetNextPosition();
}