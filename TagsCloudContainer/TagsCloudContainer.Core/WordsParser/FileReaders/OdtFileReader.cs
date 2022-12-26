using AODL.Document.Content;
using AODL.Document.TextDocuments;
using TagsCloudContainer.Core.WordsParser.Interfaces;

namespace TagsCloudContainer.Core.WordsParser.FileReaders
{
    public class OdtFileReader : IFileReader
    {
        private readonly string _filePath;

        public OdtFileReader(string filePath)
        {
            _filePath = filePath;
        }
        public IEnumerable<string> ReadWords()
        {
           using var doc = new TextDocument();
           doc.Load(_filePath);
           return doc.Content.Cast<IContent>().Select(s => s.Node.InnerText);
        }
    }
}
