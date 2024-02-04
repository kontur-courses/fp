using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using TagsCloudVisualization.Common;

namespace TagsCloudVisualization.TextReaders;

public class DocTextReader : TextReader
{
    public DocTextReader(SourceSettings settings) : base(settings)
    {
    }

    public override Result<string> GetText()
    {
        using var document = WordprocessingDocument.Open(Settings.Path, false);
        return GetBody(document)
            .Then(body => string.Join("\n", body.Descendants<Text>().Select(t => t.Text)));
    }

    private static Result<Body> GetBody(WordprocessingDocument document)
    {
        return Result.Of(() => document.MainDocumentPart.Document.Body, "Тело источника пусто. Предоставьте другой файл.");
    }
}