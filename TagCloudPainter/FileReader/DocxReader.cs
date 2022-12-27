using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using TagCloudPainter.ResultOf;

namespace TagCloudPainter.FileReader;

public class DocxReader : IFileReader
{
    public Result<IEnumerable<string>> ReadFile(string path)
    {
        if (!path.EndsWith("docx"))
            return Result.Fail<IEnumerable<string>>("file is not in docx format");

        if (!File.Exists(path))
            return Result.Fail<IEnumerable<string>>($"path {path} does not exist ");

        return Result.Of(() => WordprocessingDocument.Open(path, false), "Cannot read file").
            Then(p => p.MainDocumentPart.Document.Body.Descendants<Paragraph>()).
            Then(p => p.Select(x => x.InnerText));
    }
}