using System.Drawing;
using CSharpFunctionalExtensions;

namespace TagsCloudLibrary.Layouters
{
    public interface ILayouter
    {
        string Name { get; }
        Result<Rectangle> PutNextRectangle(Size rectangleSize);
    }
}
