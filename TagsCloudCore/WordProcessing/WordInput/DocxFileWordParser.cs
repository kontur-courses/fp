using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using TagsCloudCore.Common.Enums;

namespace TagsCloudCore.WordProcessing.WordInput;

public class DocxFileWordParser : IWordProvider
{
    public Result<string[]> GetWords(string resourceLocation)
    {
        return Result.Of(() => WordprocessingDocument.Open(resourceLocation, false))
            .Then(doc => doc.MainDocumentPart?.Document.Body)
            .Then(body => body?.Elements<Paragraph>()
                .Select(paragraph => paragraph.InnerText)
                .ToArray() ?? Result.Fail<string[]>(
                $"Failed to read from doc/docx file {resourceLocation} Most likely the file path is incorrect or the file is corrupted."));
    }

    public WordProviderType Info => WordProviderType.Docx;

    public bool Match(WordProviderType info)
    {
        return info == Info || info == WordProviderType.Doc;
    }
}