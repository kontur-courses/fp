using System.Drawing;
using TagCloud.Infrastructure.Monad;

namespace TagCloud.Infrastructure.Saver;

public interface IImageSaver
{
    Result<None> Save(Bitmap bitmap, string outputPath, string outputFormat);
}