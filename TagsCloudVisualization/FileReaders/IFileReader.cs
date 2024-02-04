using Results;

namespace TagsCloudVisualization.FileReaders;

public interface IFileReader
{
    bool CanRead(string path);
    Result<string> ReadText(string path);
}
