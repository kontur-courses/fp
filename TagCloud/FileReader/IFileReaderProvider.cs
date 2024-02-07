using ResultOf;

namespace TagCloud.FileReader;

public interface IFileReaderProvider
{
    Result<IFileReader> CreateReader(string inputPath);
}