using System.Drawing;
using TagCloudContainer.Result;

namespace TagCloudContainer.LayouterAlgorithms
{
    public interface ICloudLayouterAlgorithm
    {
        public Result<Rectangle> PutNextRectangle(Size rectangleSize);
    }
}