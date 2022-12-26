using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.IO;
using System.Text;
using TagCloud.ResultMonade;

namespace TagCloud.FileReader
{
    public class DocxFileReader : IFileReader
    {
        public Result<string> ReadAllText(string filePath)
        {
            if (!filePath.EndsWith(".doc") && !filePath.EndsWith(".docx"))
                return Result.Fail<string>($"Input file {filePath} isn't format .doc or .docx");

            if (!File.Exists(filePath))
                return Result.Fail<string>($"Input file {filePath} doesn't exist");

            var sb = new StringBuilder();

            Result<WordprocessingDocument> wordDocument = 
                Result.Of(() => WordprocessingDocument.Open(filePath, false));

            if (!wordDocument.IsSuccess)
                return Result.Fail<string>($"Input file {filePath} has invalid content");

            var paragraphs = wordDocument.Value.MainDocumentPart.RootElement.Descendants<Paragraph>();

            foreach (var paragraph in paragraphs)
                sb.AppendLine(paragraph.InnerText);

            wordDocument.Value.Close();

            return sb.ToString().Trim();
        }
    }
}