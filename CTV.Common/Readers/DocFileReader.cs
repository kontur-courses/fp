using System.IO;
using System.Linq;
using System.Text;
using FunctionalProgrammingInfrastructure;
using Spire.Doc;
using Spire.Doc.Documents;

namespace CTV.Common.Readers
{
    public class DocFileReader: IFileReader
    {
        public Result<string> ReadToEnd(Stream inputSteam)
        {
            return Result
                .Of(() => new Document(inputSteam))
                .Then(JoinParagraphs);
        }

        private static string JoinParagraphs(Document doc)
        {
            var sb = new StringBuilder(); 
            foreach (Section section in doc.Sections)
            {
                foreach (Paragraph paragraph in section.Paragraphs)
                {
                    sb.AppendLine(paragraph.Text);
                }
            }

            return sb.ToString();
        }
    }
}