using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudResult.Algorithms
{
    public interface IAlgorithm
    {
        Result<IReadOnlyDictionary<string, (Rectangle, int)>> GenerateRectanglesSet(IReadOnlyDictionary<string, int> processedWords);
    }
}