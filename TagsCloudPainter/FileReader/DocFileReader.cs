using ResultLibrary;
using Spire.Doc;

namespace TagsCloudPainter.FileReader;

public class DocFileReader : IFileReader
{
    public HashSet<string> SupportedExtensions => new() { ".doc", ".docx" };

    public Result<string> ReadFile(string path)
    {
        var document = Result.Of(() => new Document());
        var loadingResult = document.Then(doc => doc.LoadFromFile(path));
        if (!loadingResult.IsSuccess)
            return Result.Fail<string>(loadingResult.Error);

        var text = document.Then(doc => doc.GetText());
        var lastIndexOfWatermark = text.Then(text => text.IndexOf(Environment.NewLine));
        var textWithoutWatermark = lastIndexOfWatermark
            .Then(index => text.Then(text => text.Substring(index + 2).Trim()));

        return textWithoutWatermark;
    }
}