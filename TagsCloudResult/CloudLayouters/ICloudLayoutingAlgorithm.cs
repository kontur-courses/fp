using System.Drawing;

namespace TagsCloudResult.CloudLayouters
{
    public interface ICloudLayoutingAlgorithm
    {
        Rectangle PutNextRectangle(Size rectangleSize);
    }
}