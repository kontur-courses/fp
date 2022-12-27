using TagCloud.ResultImplementation;

namespace TagCloud.FileReader;

public interface IFileReader
{
    Result<string[]> Read(string path);
}