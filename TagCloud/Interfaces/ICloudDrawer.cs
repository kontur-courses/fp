using System.Collections.Generic;
using System.Drawing;
namespace TagCloud;

public interface ICloudDrawer
{
    Result<Bitmap> DrawCloud(Result<IEnumerable<WordForCloud>> wordsForCloud);
}