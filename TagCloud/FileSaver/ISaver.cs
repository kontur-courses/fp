using System.Drawing;
using ResultOf;

namespace TagCloud.FileSaver;

public interface ISaver
{
    Result<None> Save(Bitmap bitmap, string outputPath, string imageFormat);
}