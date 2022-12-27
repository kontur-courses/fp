using TagCloud.ResultImplementation;

namespace TagCloud.FileConverter;

public interface IFileConverter
{
    Result<string> Convert(string path);
}