using System.Drawing;
using System.Runtime.CompilerServices;
using ResultOf;

[assembly: InternalsVisibleTo("TagsCloud_Test")]

namespace TagCloud.Layout
{
    internal interface ICloudLayouter
    {
        Result<Rectangle> PutNextRectangle(Size rectangleSize);
        void Reset();
    }
}