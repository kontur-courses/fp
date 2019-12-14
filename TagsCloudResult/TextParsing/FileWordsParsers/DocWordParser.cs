using System.Collections.Generic;
using DocumentFormat.OpenXml.Packaging;

namespace TagsCloudResult.TextParsing.FileWordsParsers
{
    public class DocWordParser : IFileWordsParser
    {
        public IEnumerable<string> ParseFrom(string path)
        {
            using (var wordDocument = WordprocessingDocument.Open(path, false))
            {
                var body = wordDocument.MainDocumentPart.Document.Body;
                foreach (var line in body.ChildElements)
                    yield return line.InnerText;
            }
        }
    }
}