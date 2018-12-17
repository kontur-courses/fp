using System.IO;
using System.Text;
using ResultOf;

namespace TagsCloudVisualization.WordsFileReading
{
    public class SimpleFormatsReader : IFileReader
    { 
        public Result<string> ReadText(string fileName)
        {
            return Result.Of(() => ReadTextNotPure(fileName));
        }

        private string ReadTextNotPure(string fileName)
        {
            using (var reader = new StreamReader(fileName, Encoding.UTF8))
                return reader.ReadToEnd();
        }

        public string[] SupportedTypes()
        {
            return new[] {"txt", "json"};
        }
    }
}
