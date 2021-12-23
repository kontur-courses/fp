using System.Collections.Generic;
using System.Drawing;
using TagCloud2.TextGeometry;

namespace TagCloud2
{
    public class ColoredCloud : IColoredCloud
    {
        public List<IColoredSizedWord> ColoredWords { get; }

        public ColoredCloud()
        {
            ColoredWords = new();
        }

        public void AddColoredWordsFromCloudLayouter(IColoredSizedWord[] words, List<Rectangle> rectangles, IColoringAlgorithm coloringAlgorithm)
        {
            
            for (int i = 0; i < words.Length; i++)
            {
                var color = coloringAlgorithm.GetColor(rectangles[i]);
                ColoredWords.Add(new ColoredSizedWord(color, rectangles[i], words[i].Word, words[i].Font));
            }
        }
    }
}
