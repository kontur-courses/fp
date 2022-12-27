using Spire.Doc;
using Spire.Doc.Documents;
using System.Text;
using TagCloudContainer.Interfaces;
using TagCloudContainer.Result;
using static System.Net.Mime.MediaTypeNames;

namespace TagCloudContainer.Readers
{
    public class Reader : IFileReader
    {
        private static string TxtRead(string path)
        {
            return File.ReadAllText(path);
        }

        private static string DocRead(string path)
        {
            var stringBuilder = new StringBuilder();
            var document = new Document(path);

            foreach (Section section in document.Sections)
                foreach (Paragraph paragraph in section.Paragraphs)
                    stringBuilder.AppendLine(paragraph.Text);

            return stringBuilder.ToString();
        }

        public Result<string> ReadFile(string path)
        {
            try
            {
                return Result.Result.Ok(Path.GetExtension(path) switch
                {
                    ".txt" => TxtRead(path),
                    ".doc" or ".docx" => DocRead(path),
                    _ => throw new ArgumentException("Incorrect file extension: " + Path.GetExtension(path)),
                });
            }
            catch (Exception e)
            {
                return Result.Result.Fail<string>(e.Message);
            }
        }
    }
}
