using System.Drawing;
using ResultOf;

namespace HomeExercise
{
    public interface ICircularCloudLayouter
    {
        public Point Center { get; }
        Result<Rectangle> PutNextRectangle(Size rectangleSize);
    }
}