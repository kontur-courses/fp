using TagsCloudContainer.Infrastructure;

namespace TagsCloudContainer.Parsers;

public interface IParser
{
    string[] GetFormats();

    Result<IEnumerable<string>> Parse(string path);
}
