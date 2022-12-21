using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using ResultOf;
using System.IO;
using System.Text;

namespace TagCloud.FileReader
{
    public class DocxFileReader : IFileReader
    {
        public Result<string> ReadAllText(string filePath)
        {
            if (!File.Exists(filePath))
                return new Result<string>($"File {filePath} doesn't exist");

            if (Path.GetExtension(filePath) != ".doc" && Path.GetExtension(filePath) != ".docx")
                return new Result<string>($"File {filePath} has invalid format");

            var sb = new StringBuilder();

            using (WordprocessingDocument wordDocument =
                    WordprocessingDocument.Open(filePath, false))
            {
                var paragraphs = wordDocument.MainDocumentPart.RootElement.Descendants<Paragraph>();

                foreach (var paragraph in paragraphs)
                    sb.AppendLine(paragraph.InnerText);

                wordDocument.Close();

                return sb.ToString().Trim();
            }
        }
    }
}