using System.Collections.Generic;
using System.Drawing;
using ResultOf;

namespace TagsCloudContainer.Layouter
{
    public abstract class MustInitialize<T>
    {
        public MustInitialize(T parameters)
        {
        }
    }

    public interface ICloudLayouter
    {
        Result<Rectangle> PutNextRectangle(Size rectangleSize);
        List<Rectangle> RectanglesList { get; }
        Point Center { get; set; }
        IPointsGenerator PointsGenerator { get; }
    }
}