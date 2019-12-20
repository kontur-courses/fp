using System.IO;
using ResultOf;

namespace TagCloud.Infrastructure
{
    public interface IFileInfoProvider
    {
        Result<FileInfo> GetFileInfo(string path);
    }
}
