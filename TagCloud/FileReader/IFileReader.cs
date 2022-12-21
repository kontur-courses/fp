using TagCloud.ResultMonade;

namespace TagCloud.FileReader
{
    public interface IFileReader
    {
        Result<string> ReadAllText(string filePath);
    }
}
