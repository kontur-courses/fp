using System.Collections.Generic;
using System.Drawing;
using ResultOf;
using TagCloud.Infrastructure;

namespace TagCloud.Visualization
{
    public interface ITagCloudGenerator
    {
        Result<Bitmap> GetTagCloudBitmap(IEnumerable<Word> words);
    }
}
