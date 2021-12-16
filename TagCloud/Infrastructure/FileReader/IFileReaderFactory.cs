using TagCloud.Infrastructure.Monad;

namespace TagCloud.Infrastructure.FileReader;

public interface IFileReaderFactory
{
    Result<IFileReader> Create(string filePath);
}