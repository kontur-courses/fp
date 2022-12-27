using TagCloudContainer.Result;

namespace TagCloudContainer.Interfaces
{
    public interface IFileReader
    {
        Result<string> ReadFile(string path);
    }
}
