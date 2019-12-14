using System.Drawing;
using ErrorHandler;

namespace TagsCloudVisualization.Services
{
    public interface IVisualizer
    {
        Result<Bitmap> VisualizeTextFromFile(string fileName);
    }
}