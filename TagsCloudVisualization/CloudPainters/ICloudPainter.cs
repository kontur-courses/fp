using System.Collections.Generic;
using System.Drawing;
using TagsCloudVisualization.ErrorHandling;
using TagsCloudVisualization.Visualization;

namespace TagsCloudVisualization.CloudPainters
{
    public interface ICloudPainter<in T>
    {
        Result<Bitmap> GetImage(IEnumerable<T> drawnComponents, VisualisingOptions visualisingOptions);
    }
}