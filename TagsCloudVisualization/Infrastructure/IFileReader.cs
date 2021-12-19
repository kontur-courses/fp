using System.IO;

namespace TagsCloudVisualization.Infrastructure
{
    public interface IFileReader
    {
        bool CanReadFile(FileInfo file);
        Result<string> ReadFile(FileInfo file);
    }
}