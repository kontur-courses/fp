using Result;

namespace TagCloud;

public interface IFileLoader
{
    public Result<string> Load(string path);
}