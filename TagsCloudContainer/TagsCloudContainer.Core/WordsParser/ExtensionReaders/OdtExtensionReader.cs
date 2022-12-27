using AODL.Document.Content;
using AODL.Document.TextDocuments;
using TagsCloudContainer.Core.WordsParser.Interfaces;

namespace TagsCloudContainer.Core.WordsParser.ExtensionReaders
{
    public class OdtExtensionReader : IExtensionReader
    {
        private readonly string _filePath;

        public OdtExtensionReader(string filePath)
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
