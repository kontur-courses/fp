using ResultOf;

namespace TagCloud2
{
    public interface IFileReader
    {
        Result<string> ReadFile(string path);
    }
}
