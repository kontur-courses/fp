using TagCloud.Infrastructure.Monad;

namespace TagCloud.Infrastructure.FileReader;

public interface IFileReader
{
    Result<IEnumerable<string>> GetLines(string inputPath);

    IEnumerable<string> GetSupportedExtensions();
}