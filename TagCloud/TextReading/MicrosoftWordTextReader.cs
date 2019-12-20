using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using ResultOf;


namespace TagCloud.TextReading
{
    public class MicrosoftWordTextReader : ITextReader
    {
        public Result<IEnumerable<string>> ReadWords(FileInfo file)
        {
            Body body;
            try
            {
                using (var wordDocument = WordprocessingDocument.Open(file.FullName, false))
                {
                    body = wordDocument.MainDocumentPart.Document.Body;
                }
            }
            catch (IOException e)
            {
                return Result
                    .Fail<IEnumerable<string>>(e.Message)
                    .RefineError($"File {file.FullName} is in use");
            }

            return Result.Ok(ExtractWordsFromBody(body));
        }

        private IEnumerable<string> ExtractWordsFromBody(Body body)
        {
            return body.ChildElements.Select(element => element.InnerText).Where(text => text != "");
        }

        public string Extension => ".docx";
    }
}
