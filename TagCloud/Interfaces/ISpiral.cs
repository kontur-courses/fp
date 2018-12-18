using TagCloud.Layouter;
using TagCloud.Result;

namespace TagCloud.Interfaces
{
    public interface ISpiral
    {
        Result<Point> Put(Point origin, double angle, double turnsInterval);
    }
}