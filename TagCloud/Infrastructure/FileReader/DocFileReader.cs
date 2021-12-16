using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using TagCloud.Infrastructure.Monad;

namespace TagCloud.Infrastructure.FileReader;

public class DocFileReader : IFileReader
{
    private static readonly IReadOnlySet<string> SupportedExtensions = new HashSet<string> { ".docx", ".doc" };

    public Result<IEnumerable<string>> GetLines(string inputPath)
    {
        return IsValidInputPath(inputPath, out var error)
            ? Result.Ok(ReadLines(inputPath))
            : Result.Fail<IEnumerable<string>>(error);
    }

    public virtual IEnumerable<string> GetSupportedExtensions()
    {
        return SupportedExtensions;
    }

    protected virtual bool IsSupportedExtension(string extension)
    {
        return SupportedExtensions.Contains(extension);
    }

    private static IEnumerable<string> ReadLines(string inputPath)
    {
        using var document = WordprocessingDocument.Open(inputPath, false);
        var paragraphs = document.MainDocumentPart.RootElement;

        return paragraphs.Descendants<Paragraph>().Select(x => x.InnerText);
    }

    private bool IsValidInputPath(string inputPath, out string error)
    {
        if (!File.Exists(inputPath))
        {
            error = $"The file does not exist {inputPath}";
            return false;
        }

        var extension = new FileInfo(inputPath).Extension;

        if (!IsSupportedExtension(extension))
        {
            error = $"Extension {extension} is not supported";
            return false;
        }

        error = string.Empty;
        return true;
    }
}