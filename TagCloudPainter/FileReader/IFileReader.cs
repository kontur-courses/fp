using TagCloudPainter.ResultOf;

namespace TagCloudPainter.FileReader;

public interface IFileReader
{
    public Result<IEnumerable<string>> ReadFile(string path);
}