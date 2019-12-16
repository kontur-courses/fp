using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudResult.CloudVisualizers
{
    public interface ICloudVisualizer
    {
        Result<Bitmap> GetBitmap(IEnumerable<CloudVisualizationWord> words);
    }
}