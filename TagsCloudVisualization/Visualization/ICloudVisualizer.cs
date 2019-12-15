using System.Collections.Generic;
using System.Drawing;
using TagsCloudVisualization.CloudPainters;
using TagsCloudVisualization.ErrorHandling;
using TagsCloudVisualization.Layouters;

namespace TagsCloudVisualization.Visualization
{
    public interface ICloudVisualizer<T>
    {
        Result<Bitmap> GetVisualization(IEnumerable<string> words, ILayouter layouter,
            ICloudPainter<T> cloudPainter);
    }
}