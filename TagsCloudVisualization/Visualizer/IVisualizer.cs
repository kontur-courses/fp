using System.Drawing;
using ResultOf;

namespace TagsCloudVisualization.Visualizer
{
    public interface IVisualizer<T>
    {
        Result<Bitmap> Draw();
    }
}