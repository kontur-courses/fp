using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using TagCloudPainter.ResultOf;
using TagCloudPainter.Extensions;

namespace TagCloudPainter.FileReader;

public class DocxReader : IFileReader
{
    public Result<IEnumerable<string>> ReadFile(string path)
    {
        return path.ValidatePath("docx").
            Then(path => WordprocessingDocument.Open(path,false)).
            Then(p => p.MainDocumentPart.Document.Body.Descendants<Paragraph>()).
            Then(p => p.Select(x => x.InnerText));
    }
}