using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using TagCloud.Infrastructure.Monad;

namespace TagCloud.Infrastructure.FileReader;

public class DocFileReader : IFileReader
{
    private static readonly IReadOnlySet<string> SupportedExtensions = new HashSet<string> { ".docx", ".doc" };

    public IEnumerable<string> GetSupportedExtensions()
    {
        return SupportedExtensions;
    }

    public Result<IEnumerable<string>> GetLines(string inputPath)
    {
        if (!File.Exists(inputPath))
            return Result.Fail<IEnumerable<string>>($"The file does not exist {inputPath}");

        var extension = new FileInfo(inputPath).Extension;

        if (!SupportedExtensions.Contains(extension))
            return Result.Fail<IEnumerable<string>>($"Extension {extension} is not supported");

        using var document = WordprocessingDocument.Open(inputPath, false);
        var paragraphs = document.MainDocumentPart.RootElement;

        return Result.Ok(paragraphs.Descendants<Paragraph>().Select(x => x.InnerText));
    }
}