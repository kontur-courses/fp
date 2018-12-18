using System.Collections.Generic;
using System.Drawing;
using TagCloud.Data;

namespace TagCloud.WordsLayouter
{
    public interface IWordsLayouter
    {
        Result<IEnumerable<WordImageInfo>> GenerateLayout(IEnumerable<WordInfo> words, FontFamily fontFamily, int fontMultiplier);
    }
}