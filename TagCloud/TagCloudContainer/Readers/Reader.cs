using Spire.Doc;
using Spire.Doc.Documents;
using System.Text;
using Spire.Doc.Interface;

namespace TagCloudContainer.Readers
{
    public class Reader : IFileReader
    {
        public Result<string> TxtRead(string path)
        {
            return Result.Of(()=>File.ReadAllText(path))
                .RefineError("Error file path");
        }
        
        public Result<string> DocRead(string path)
        {
            return Result.Of(() => new Document(path))
                .Then(GetTextFromDoc)
                .RefineError("Error file path");
        }
        private static string GetTextFromDoc(IDocument document)
        {
            var stringBuilder = new StringBuilder();
            foreach (Section section in document.Sections)
            foreach (Paragraph paragraph in section.Paragraphs)
                stringBuilder.AppendLine(paragraph.Text);
            return stringBuilder.ToString();
        }
    }
}
