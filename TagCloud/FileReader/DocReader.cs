using ResultOf;
using Spire.Doc;

namespace TagCloud.FileReader;

public class DocReader : IFileReader
{
    public IEnumerable<string> GetAvailableExtensions() => new List<string>() { "doc", "docx" };

    public Result<IEnumerable<string>> ReadLines(string inputPath)
    {
        var fileExistsResult = FileReaderUtils.FileExists(inputPath);

        return fileExistsResult.IsSuccess
            ? Result.Ok(ReadFile(inputPath))
            : Result.Fail<IEnumerable<string>>(fileExistsResult.Error);
    }

    private IEnumerable<string> ReadFile(string inputPath)
    {
        using var document = new Document(inputPath, FileFormat.Auto);
        var text = document.GetText();

        return text.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Skip(1);
    }

    private bool FileExists(string inputPath, out string error)
    {
        if (!File.Exists(inputPath))
        {
            error = $"Input file {inputPath} doesn't exist";
            return false;
        }

        error = string.Empty;
        return true;
    }
}