using System;
using DocumentFormat.OpenXml.Packaging;
using TagsCloud.ResultPattern;

namespace TagsCloud.FileReader
{
    public class DocxReader : IWordsReader
    {
        public Result<string[]> ReadWords(string path)
        {
            return path.AsResult()
                .Then(x => WordprocessingDocument.Open(x, false))
                .Then(x => x.MainDocumentPart.Document.Body.InnerText)
                .Then(x => x.Split(new string[0], StringSplitOptions.RemoveEmptyEntries))
                .RefineError("with path:\n" + path);
        }
    }
}