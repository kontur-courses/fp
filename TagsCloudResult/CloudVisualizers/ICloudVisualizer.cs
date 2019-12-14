using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudResult.CloudVisualizers
{
    public interface ICloudVisualizer
    {
        Bitmap GetBitmap(IEnumerable<CloudVisualizationWord> words);
    }
}