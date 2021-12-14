namespace TagsCloudContainer.Interfaces;

public interface IParser
{
    string[] GetFormats();

    Result<IEnumerable<string>> Parse(string path);
}
