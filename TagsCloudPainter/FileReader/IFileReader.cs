using ResultLibrary;

namespace TagsCloudPainter.FileReader;

public interface IFileReader
{
    public HashSet<string> SupportedExtensions { get; }
    public Result<string> ReadFile(string path);
}