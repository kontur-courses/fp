using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudResult.CloudVisualizers
{
    public interface IBitmapMaker
    {
        Result<Bitmap> MakeBitmap(IEnumerable<CloudVisualizationWord> words, CloudVisualizerSettings settings);
    }
}