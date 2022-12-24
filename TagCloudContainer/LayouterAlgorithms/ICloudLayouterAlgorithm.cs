using System.Drawing;
using TagCloudContainer.TaskResult;

namespace TagCloudContainer.LayouterAlgorithms
{
    public interface ICloudLayouterAlgorithm
    {
        public Result<Rectangle> PutNextRectangle(Size rectangleSize);
    }
}