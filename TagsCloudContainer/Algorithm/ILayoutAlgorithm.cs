using System.Collections.Generic;
using System.Drawing;
using ResultOf;

namespace TagsCloudContainer.Algorithm
{
    public interface ILayoutAlgorithm
    {
        Result<IEnumerable<(string, Rectangle)>> GetLayout(IEnumerable<string> words, Size pictureSize);
    }
}