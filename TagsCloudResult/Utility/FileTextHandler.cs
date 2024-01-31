using ResultSharp;
using Spire.Doc;

namespace TagsCloudResult.Utility;

public class FileTextHandler : ITextHandler
{
    public Result<string> ReadText(string filePath)
    {
        if (!File.Exists(filePath)) return Result<string>.Err($"Source file {filePath} doesn't exist");

        if (filePath.EndsWith(".doc") || filePath.EndsWith(".docx"))
        {
            var document = new Document();
            document.LoadFromFile(filePath);
            var text = document.GetText();
            return Result<string>.Ok(text[(text.IndexOf('\n') + 1)..].Trim());
        }

        var result = Result.Try(() => File.ReadAllText(filePath));
        return result.IsOk ? Result.Ok(result.Unwrap()) : Result<string>.Err($"Source file {filePath} doesn't accessible");
    }
}