using System.Drawing;
using CSharpFunctionalExtensions;

namespace TagsCloudContainer.Layout
{
    public interface IRectangleLayout
    {
        Result<Rectangle> PutNextRectangle(Size rectangleSize);
    }
}