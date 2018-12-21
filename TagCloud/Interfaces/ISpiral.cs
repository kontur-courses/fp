using TagCloud.Layouter;

namespace TagCloud.Interfaces
{
    public interface ISpiral
    {
        Result<Point> Put(Point origin, double angle, double turnsInterval);
    }
}