using DocumentFormat.OpenXml.Packaging;
using TagsCloudContainer.Core.WordsParser.Interfaces;

namespace TagsCloudContainer.Core.WordsParser.ExtensionReaders
{
    public class DocxReader : IExtensionReader
    {
        private readonly string _filePath;

        public DocxReader(string filePath)
        {
            _filePath = filePath;
        }

        public IEnumerable<string> ReadWords()
        {
            using var doc = WordprocessingDocument.Open(_filePath, true);
            var words = doc.MainDocumentPart?.Document.Body?.Select(p => p.InnerText);
            doc.Close();

            return words ?? new List<string>();
        }
    }
}
