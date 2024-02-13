using ResultSharp;
using Spire.Doc;

namespace TagsCloudResult.Utility;

public class FileTextHandler : ITextHandler
{
    public MyResult<string> ReadText(string filePath)
    {
        if (!File.Exists(filePath))
            return MyResult<string>.Err($"Source file {filePath} doesn't exist");

        if (filePath.EndsWith(".doc") || filePath.EndsWith(".docx"))
        {
            var document = new Document();
            var textResult = Result.Try(() =>
            {
                document.LoadFromFile(filePath);
                return document.GetText();
            });

            if (textResult.IsErr)
                return MyResult<string>.Err($"Spire.Doc library exception:\n{textResult.UnwrapErr()}");

            var text = textResult.Unwrap();
            return MyResult<string>.Ok(text[(text.IndexOf('\n') + 1)..].Trim());
        }

        var result = Result.Try(() => File.ReadAllText(filePath));
        return result.IsOk
            ? MyResult.Ok(result.Unwrap())
            : MyResult<string>.Err($"Source file {filePath} doesn't accessible");
    }
}