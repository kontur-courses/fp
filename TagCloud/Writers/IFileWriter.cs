using System.Drawing;
using TagCloud.ResultMonad;

namespace TagCloud.Writers
{
    public interface IFileWriter
    {
        Result<None> Write(Bitmap bitmap, string filename, string extension, string targetDirectory);
    }
}
