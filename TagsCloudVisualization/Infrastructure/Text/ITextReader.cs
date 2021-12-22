using System.IO;

namespace TagsCloudVisualization.Infrastructure.Text
{
    public interface ITextReader
    {
        bool CanReadFile(FileInfo file);
        Result<string> ReadFile(FileInfo file);
    }
}