using TagsCloudContainer.Core.WordsParser.Interfaces;

namespace TagsCloudContainer.Core.WordsParser.ExtensionReaders
{
    public class TxtReader : IExtensionReader
    {
        private readonly string _filePath;

        public TxtReader(string filePath)
        {
            _filePath = filePath;
        }

        public IEnumerable<string> ReadWords()
        {
            return File.ReadLines(_filePath);
        }
    }
}