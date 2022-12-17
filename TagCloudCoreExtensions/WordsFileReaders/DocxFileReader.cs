using DocumentFormat.OpenXml.Packaging;
using TagCloudCore.Interfaces.Providers;

namespace TagCloudCoreExtensions.WordsFileReaders;

public class DocxFileReader : StandardFileReader
{
    public DocxFileReader(IWordsPathSettingsProvider pathSettingsProvider) : base(pathSettingsProvider)
    {
    }

    public override string SupportedExtension => ".docx";

    protected override string InternalReadFile(string path)
    {
        using var wordDoc = WordprocessingDocument.Open(path, false);
        var body = wordDoc.MainDocumentPart!.Document.Body!;
        var lines = body.ChildElements.Select(line => line.InnerText);
        return string.Join("\r\n", lines);
    }
}