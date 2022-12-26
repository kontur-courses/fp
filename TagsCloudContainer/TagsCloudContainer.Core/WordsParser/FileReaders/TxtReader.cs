using TagsCloudContainer.Core.WordsParser.Interfaces;

namespace TagsCloudContainer.Core.WordsParser.FileReaders
{
    public class TxtReader : IFileReader
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