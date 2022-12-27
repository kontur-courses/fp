using System.Text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using TagCloudPainter.ResultOf;

namespace TagCloudPainter.FileReader;

public class PdfFileReader : IFileReader
{
    public Result<IEnumerable<string>> ReadFile(string path)
    {
        if (!path.EndsWith("pdf"))
            return Result.Fail<IEnumerable<string>>("file is not in pdf format");

        if (!File.Exists(path))
            return Result.Fail<IEnumerable<string>>($"path {path} does not exist ");

        using (var reader = new PdfReader(path))
        {
            var sb = new StringBuilder();
            for (var i = 1; i <= reader.NumberOfPages; i++)
            {
                var text = PdfTextExtractor.GetTextFromPage(reader, i, new LocationTextExtractionStrategy());
                sb.Append(text);
            }

            return sb.ToString().Replace(" ", "").Split('\n').Where(x => x != "").AsResult();
        }
    }
}