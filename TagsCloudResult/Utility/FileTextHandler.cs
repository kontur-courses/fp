using Spire.Doc;

namespace TagsCloudResult.Utility;

public class FileTextHandler : ITextHandler
{
    public string ReadText(string filePath)
    {
        if (filePath.EndsWith(".doc") || filePath.EndsWith(".docx"))
        {
            var document = new Document();
            document.LoadFromFile(filePath);
            var text = document.GetText();
            return text[(text.IndexOf('\n') + 1)..].Trim();
        }

        return File.ReadAllText(filePath);
    }
}