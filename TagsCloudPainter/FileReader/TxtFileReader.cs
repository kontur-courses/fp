using ResultLibrary;

namespace TagsCloudPainter.FileReader;

public class TxtFileReader : IFileReader
{
    public HashSet<string> SupportedExtensions => new() { ".txt" };

    public Result<string> ReadFile(string path)
    {
        var text = Result.Of(() => File.ReadAllText(path)).Then(text => text.Trim());

        return text;
    }
}