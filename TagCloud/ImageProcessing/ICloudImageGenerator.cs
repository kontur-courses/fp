using System.Collections.Generic;
using System.Drawing;
using TagCloud.ResultMonade;

namespace TagCloud.ImageProcessing
{
    public interface ICloudImageGenerator
    {
        Result<Bitmap> GenerateBitmap(IReadOnlyDictionary<string, double> wordsFrequencies);
    }
}
