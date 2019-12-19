using System.Drawing;
using TagCloudGenerator.ResultPattern;

namespace TagCloudGenerator.GeneratorCore.CloudLayouters
{
    public interface ICloudLayouter
    {
        Result<Rectangle> PutNextRectangle(Size rectangleSize);
    }
}