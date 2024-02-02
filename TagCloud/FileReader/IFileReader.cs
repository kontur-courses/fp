using ResultOf;

namespace TagCloud.FileReader;

public interface IFileReader
{
    Result<IEnumerable<string>> ReadLines(string inputPath);

    IList<string> GetAvailableExtensions();
}