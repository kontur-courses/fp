using System.Collections.Generic;
using System.Drawing;
using TagCloud2.TextGeometry;

namespace TagCloud2
{
    public interface IColoredCloud
    {
        List<IColoredSizedWord> ColoredWords { get; }

        void AddColoredWordsFromCloudLayouter(IColoredSizedWord[] words, List<Rectangle> rectangles, IColoringAlgorithm coloringAlgorithm);
    }
}
