using DocumentFormat.OpenXml.Packaging;

namespace TagCloudGenerator.TextReaders
{
    public class DocxReader : ITextReader
    {
        public string GetFileExtension() => ".docx";

        Result<IEnumerable<string>> ITextReader.ReadTextFromFile(string filePath)
        {
            try
            {
                using (WordprocessingDocument wordDocument =
                  WordprocessingDocument.Open(filePath, false))
                {
                    var body = wordDocument.MainDocumentPart.Document.Body;
                    var paragraphs = body.ChildElements;

                    var text = new List<string>(paragraphs.Count);
                    foreach (var paragraph in paragraphs)
                    {
                        if (paragraph.InnerText != "")
                            text.Add(paragraph.InnerText);
                    }

                    return new Result<IEnumerable<string>>(text, null);
                }
            }
            catch
            {
                return new Result<IEnumerable<string>>(null, $"Could not find file {filePath}");
            }
        }
    }
}