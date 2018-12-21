using System.Collections.Generic;
using System.Drawing;
using ResultOf;

namespace TagCloud
{
    public interface ICloudLayouter
    {
        Result<CloudLayouter> AddWordsFromDictionary(Dictionary<string, int> words);
        IReadOnlyDictionary<Rectangle, string> Rectangles { get; }
    }
}