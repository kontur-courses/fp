using System.Collections.Generic;
using System.Drawing;
using TagsCloudVisualization.ResultOf;

namespace TagsCloudVisualization.Visualization
{
    public interface IVisualizer
    {
        public Result<Bitmap> Visualize(IEnumerable<string> visualizingValues);
    }
}