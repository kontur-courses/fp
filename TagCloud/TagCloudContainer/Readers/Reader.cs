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
            if (!File.Exists(path))
                return new Result<string>($"Could not find any file for create cloud:{path}.");
            return Result.Of(()=>File.ReadAllText(path))
                .RefineError("Could not read file");
        }
        
        public Result<string> DocRead(string path)
        {
            if (!File.Exists(path))
                return new Result<string>($"Could not find any file for create cloud:{path}.");
            return Result.Of(() => new Document(path))
                .Then(GetTextFromDoc)
                .RefineError("Could not read file");
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
