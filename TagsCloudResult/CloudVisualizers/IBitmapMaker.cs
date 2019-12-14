using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudResult.CloudVisualizers
{
    public interface IBitmapMaker
    {
        Bitmap MakeBitmap(IEnumerable<CloudVisualizationWord> words, CloudVisualizerSettings settings);
    }
}