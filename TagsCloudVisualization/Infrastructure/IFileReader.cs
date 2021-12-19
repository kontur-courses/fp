using System.IO;

namespace TagsCloudVisualization.Infrastructure
{
    public interface IFileReader
    {
        bool CanReadFile(FileInfo file);
        string ReadFile(FileInfo file);
    }
}