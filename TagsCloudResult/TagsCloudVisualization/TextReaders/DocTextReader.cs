using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using TagsCloudVisualization.Common;
using TagsCloudVisualization.Common.ResultOf;

namespace TagsCloudVisualization.TextReaders;

public class DocTextReader : ITextReader
{
    private readonly SourceSettings settings;
    
    public DocTextReader(SourceSettings settings)
    {
        this.settings = settings;
    }

    public Result<string> GetText()
    {
        using var document = WordprocessingDocument.Open(settings.Path, false);
        return GetBody(document)
            .Then(body => string.Join("\n", body.Descendants<Text>().Select(t => t.Text)));
    }

    private static Result<Body> GetBody(WordprocessingDocument document)
    {
        return Result.Of(() => document.MainDocumentPart.Document.Body, 
            "Тело источника пусто. Пожалуйста, предоставьте другой файл.");
    }
}
