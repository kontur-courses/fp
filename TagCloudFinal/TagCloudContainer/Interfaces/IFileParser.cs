using TagCloudContainer.Result;

namespace TagCloudContainer.Interfaces
{
    public interface IFileParser
    {
        Result<IEnumerable<string>> Parse(string text);
    }
}
