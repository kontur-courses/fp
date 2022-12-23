using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public interface ICloudCreator
    {
        Result<List<Bitmap>> Create(IEnumerable<string> words, int amountWords = -1, int amountClouds = 1);
    }
}