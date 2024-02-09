using System.Drawing;

namespace TagCloudResult.Layouter
{
    public interface IPointGenerator
    {
        public Point GetNextPoint();
        public Point CenterPoint { get; }
    }
}
