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
            if (!File.Exists(filePath))
                return Result.Fail<string>($"File {filePath} doesn't exist");

            if (Path.GetExtension(filePath) != ".doc" && Path.GetExtension(filePath) != ".docx")
                return Result.Fail<string>($"File {filePath} isn't format .doc or .docx");

            var sb = new StringBuilder();

            using (WordprocessingDocument wordDocument =
                    WordprocessingDocument.Open(filePath, false))
            {
                var paragraphs = wordDocument.MainDocumentPart.RootElement.Descendants<Paragraph>();

                foreach (var paragraph in paragraphs)
                    sb.AppendLine(paragraph.InnerText);

                return sb.ToString().Trim();
            }
        }
    }
}